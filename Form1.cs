using Busy_Light.Device_Events;
using Microsoft.Win32;
using RingCentral;
using RingCentral.Net.AuthorizeUri;
using RingCentral.Net.WebSocket;
using ServiceStack.Text.Common;
using Sprache;
using System.Diagnostics;
using System.IO.Ports;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Text.Json;
using static rc_program.Program;

namespace Busy_Light
{
    public partial class Form1 : Form
    {
        private readonly RestClient _restClient;
        private string RedirectUri = Environment.GetEnvironmentVariable("REDIRECT_URI");
        private DeviceWatcher watcher;
       
        public static event Action<byte> OnBrightnessReceived;
        public static string FindDevicePort()
        {
            foreach (string port in SerialPort.GetPortNames())
            {
                try
                {
                    using (SerialPort sp = new SerialPort(port, 9600))
                    {
                        sp.ReadTimeout = 500;
                        sp.WriteTimeout = 500;

                        sp.Open();
                        Thread.Sleep(300); // Allow ESP reset

                        sp.DiscardInBuffer();

                        sp.Write(new byte[] { 0x55 }, 0, 1);
                        Thread.Sleep(200);

                        int response = sp.ReadByte();

                        if (response == 0xAA)
                        {
                            sp.Close();
                            return port; // ✅ Correct device
                        }
                    }
                }
                catch
                {
                    // Ignore invalid ports
                }
            }

            return null;
        }
        
        public string textBox1_Text
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }
        public bool UpdateConnectionStatus()
        {
            if (SerialPortScanner.IsConnected)
            {
                textBox1.Text = "Connected";
                textBox1.BackColor = Color.Green;
                return true;
            }
            else
            {
                textBox1.Text = "Disconnected";
                textBox1.BackColor = Color.Red;
                return false;
            }
        }
        

        
        private Busy_Light.ServiceSettings _settingsService;


        private void btnExit_Click(object sender, EventArgs e)
        {
            MinimizeToTray();

        }
        private void TryConnectExistingDevice()
        {
            var ports = SerialPort.GetPortNames();

            foreach (var port in ports)
            {
                // Optional: filter if needed
                SerialPortScanner.StartComListener(port, this);
                break; // remove if you want to try all
            }
        }
        public Form1(RestClient restClient, TokenService tokenService, string redirectUri)
        {
            
            TryConnectExistingDevice();
            this.redirectUri = redirectUri;
            _settingsService = new Busy_Light.ServiceSettings();
            InitializeComponent();
            _restClient = restClient;
            _tokenService = tokenService;
            trackBar1.Scroll += trackBar1_Scroll;
            trackBar1.Value = _settingsService.Settings.Brightness; // start at max (100%)
            checkBox1.Checked = _settingsService.Settings.StartWithWindows;
           
            
            // Update label initially
            UpdateBrightnessLabel(trackBar1.Value);

        }



        public void UpdateBrightnessLabel(int value)
        {
            var settingsService = new Busy_Light.ServiceSettings()  ;
            int percent = (value * 100) / 255;
            label1.Text = $"Brightness: {percent}%";
            _settingsService.Settings.Brightness = value;
            SerialPortScanner.SendBrightnessToArduino(value); // send the raw 0-255 value
            _settingsService.Save();
        }
        private WebSocketExtension _wsExtension;
        private string redirectUri;

        private async Task StartWebSocket()
        {

            _wsExtension = new WebSocketExtension(
                new RingCentral.Net.WebSocket.WebSocketOptions
                {
                    debugMode = true
                });

            await _restClient.InstallExtension(_wsExtension);
            System.Diagnostics.Debug.WriteLine("Subscribing to presence changes...");
            await _wsExtension.Subscribe(

                new[] { "/restapi/v1.0/account/~/extension/~/presence" },
                async message =>
                {

                    await Status(message);
                    Log($"Received presence message: {message}");
                });
        }
        private async Task SMSWS()
        {
            _wsExtension = new WebSocketExtension(
                new RingCentral.Net.WebSocket.WebSocketOptions
                {
                    debugMode = true
                });
            await _restClient.InstallExtension(_wsExtension);
            await _wsExtension.Subscribe(
                new[] { "/restapi/v1.0/account/~/extension/~/message-store" },
                async message =>
                {
                    System.Diagnostics.Debug.WriteLine($"SMS Message: {message}");
                });
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {


        }
        
        private async void Form1_Load(object sender, EventArgs e)

        {
            
            watcher = new DeviceWatcher();
            label7.Visible = false;
            System.Diagnostics.Debug.WriteLine("InFormLoad");
            var token = _tokenService.Load();
            if (token != null)
            {
                try
                {
                    _restClient.token = TokenService.refresh_token; // <-- important!

                    System.Diagnostics.Debug.WriteLine("Existing token found, attempting to refresh...");

                    await _restClient.Get("/restapi/v1.0/account/~/extension/~/presence");
                    Log("Token is valid, presence API call successful.");
                    await this.StartWebSocket();

                }
                catch (Exception ex)
                {
                    Log($"Token refresh failed: {ex.Message}");
                    _tokenService.Clear();
                    Log(ex.ToString());
                    Log("Cleared invalid token.");
                    await loginProc(); // also good to retry login here
                }

            }
            
            comboBox1.Items.Add("Available");
            comboBox1.Items.Add("Unavailable");
            combox2();

        }
        private void colors()
        {
            string[] coloroptions = new string[] { "Red", "Green", "Blue", "Yellow", "Purple" };
            foreach (string color in coloroptions)
            {
                comboBox2.Items.Add(color);
                comboBox3.Items.Add(color);
                comboBox4.Items.Add(color);
            }
            
        }
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            UpdateBrightnessLabel(trackBar1.Value);

        }
        private void form1_show_hide()
        {

            Control[] controls = new Control[]
   {
        comboBox1, checkBox1, label1, label2, checkBox2,
        button1, trackBar1, label4, label3, label5,
        label6, comboBox3, comboBox4, comboBox2,
   };
            foreach (var control in controls)
            {
                control.Visible = false;
            }


        }
        private void form1_show_show()
        {

            Control[] controls = new Control[]
   {
        comboBox1, checkBox1, label1, label2, checkBox2,
        trackBar1, label4, label3, label5,
        label6, comboBox3, comboBox4, comboBox2,
   };
            foreach (var control in controls)
            {
                control.Visible = true;
            }


        }
        private void label1_Click(object sender, EventArgs e)
        {


        }
        private void combox2() //OnCall Lighting
        {
            colors();
        }
        public string ocColor { get; set; }
        private void combox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem == null)
                return;
            string selectedColor = comboBox2.SelectedItem.ToString();

            byte command = selectedColor switch
            {
                "Red" => 0x10,
                "Green" => 0x11,
                "Blue" => 0x12,
                "Yellow" => 0x13,
                "Purple" => 0x14,
                _ => 0x10 // default to Red
            };
            selectedColor = ocColor;
            try
            {
                if (SerialPortScanner.IsConnected)
                {
                    SerialPortScanner._serialPort.Write(new byte[] { command }, 0, 1);
                    SerialPortScanner._serialPort.BaseStream.Flush();
                    Debug.WriteLine($"Sent color command: {command:X2} for {selectedColor}");
                }
                else
                {
                    Debug.WriteLine("Cannot send color command — COM not started.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to send color command: {ex.Message}");
            }
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // This will prevent any key from being typed into the text portion
            e.Handled = true;
        }
        private void label2_Click(object sender, EventArgs e)
        {

        }
        public async Task<string> GetAuthorizeUrl(RestClient restClient)
        {

            var authExt = new AuthorizeUriExtension();
            await restClient.InstallExtension(authExt);

            var authorizeUri = authExt.BuildUri(new AuthorizeRequest
            {
                redirect_uri = RedirectUri,
                response_type = "code",
                scope = "ReadPresence"
            });

            return authorizeUri;
        }
        private readonly TokenService _tokenService;
        // Make this a field of your form
        private NotifyIcon trayIcon;
        private bool reallyClose = false;
        private void InitializeTrayIcon()
        {
            if (trayIcon == null)
            {

                trayIcon = new NotifyIcon
                {

                    Icon = this.Icon,
                    Visible = true,
                    Text = "Busy Light"

                };

                var contextMenu = new ContextMenuStrip();
                var exitItem = new ToolStripMenuItem("Exit");
                exitItem.Click += ExitItem_Click;
                contextMenu.Items.Add(exitItem);

                trayIcon.ContextMenuStrip = contextMenu;

                // Optional: double-click restores the window
                trayIcon.DoubleClick += (s, e) =>
                {
                    this.Show();
                    this.WindowState = FormWindowState.Normal;
                    this.ShowInTaskbar = true;

                };

            }
        }
        public void MinimizeToTray()
        {
            InitializeTrayIcon();
            this.Hide(); // Hides the form
            this.ShowInTaskbar = false;
            this.WindowState = FormWindowState.Minimized;
            Icon = this.Icon; // Set a default icon
            notifyIcon1.BalloonTipTitle = "Application Minimized";
            notifyIcon1.BalloonTipText = "Double click the icon to restore.";
            notifyIcon1.ShowBalloonTip(1000); // Show for 1 second

        }
        private void ExitItem_Click(object sender, EventArgs e)
        {
            // Allow the form to actually close
            reallyClose = true;
            this.Close();
        }
        private void Log(string message)
        {
           string path = Path.Combine(
                     Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                     "RedDenSoftware",
                     "BusyLight", "app.log");

            File.AppendAllText(path,
                $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}{Environment.NewLine}");
        }

        private static HttpListener _listener;
        private static bool _isLoggingIn = false;

        public async Task loginProc()
        {
            if (_isLoggingIn)
                return;

            _isLoggingIn = true;

            try
            {
                // Get URL ONCE
                var authUrl = await GetAuthorizeUrl(_restClient);
                System.Diagnostics.Debug.WriteLine($"Auth URL: {authUrl}"); // Log the URL for debugging
                // Open browser
                System.Diagnostics.Process.Start(new ProcessStartInfo
                {
                    FileName = authUrl,
                    UseShellExecute = true
                });

                // Clean up any previous listener
                if (_listener != null)
                {
                    try
                    {
                        if (_listener.IsListening)
                            _listener.Stop();

                        _listener.Close();
                    }
                    catch { }
                    finally
                    {
                        _listener = null;
                    }
                }

                _listener = new HttpListener();
                _listener.Prefixes.Add("http://localhost:5000/oauth2callback/");
                _listener.Start();

                var context = await _listener.GetContextAsync();
                var code = context.Request.QueryString["code"];

                // Respond to browser
                var responseString = "<html><body>You may close this window.</body></html>";
                var buffer = Encoding.UTF8.GetBytes(responseString);
                context.Response.ContentLength64 = buffer.Length;
                await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                context.Response.Close();

                if (!string.IsNullOrEmpty(code))
                {
                    try
                    {
                        Log("Authorization code received.");

                        await _restClient.Authorize(code, RedirectUri);
                        Log("Authorization successful.");

                        _tokenService.Save(_restClient.token);
                        Log("Token saved.");

                        MessageBox.Show("Login successful!");

                        await _restClient.Get("/restapi/v1.0/account/~/extension/~/presence");
                        Log("Presence API call successful.");

                        await StartWebSocket();
                        Log("WebSocket started.");

                        SerialPortScanner.ManualStatusChangeA();
                        Log("ManualStatusChangeA triggered.");
                    }
                    catch (Exception ex)
                    {
                        string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app.log");

                        Log($"ERROR: {ex.Message}");
                        Log($"STACK: {ex.StackTrace}");

                        var result = MessageBox.Show(
                            $"An error occurred.\n\nLog file:\n{logPath}\n\n" +
                            "Click YES to view the log.\n" +
                            "Click NO to restart the application.",
                            "Error",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Error);

                        if (result == DialogResult.Yes)
                        {
                            try
                            {
                                System.Diagnostics.Process.Start("notepad.exe", logPath);
                            }
                            catch
                            {
                                MessageBox.Show("Could not open log file.");
                            }
                        }
                        else if (result == DialogResult.No)
                        {
                            Application.Restart();
                        }
                    }
                }
            }
            finally
            {
                // ALWAYS release listener (this fixes your error 183)
                if (_listener != null)
                {
                    try
                    {
                        _listener.Stop();
                        _listener.Close();
                    }
                    catch { }
                    _listener = null;
                }

                _isLoggingIn = false;
            }
        }

        public async void btnLogin_Click(object sender, EventArgs e)
        {


            await loginProc();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SerialPortScanner serialPortScanner = new SerialPortScanner();
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Please select an option first.");
                return;
            }
            
            switch (comboBox1.SelectedItem.ToString())
            {
                case "Available":
                    SerialPortScanner.ManualStatusChangeA();
                    break;

                case "Unavailable":
                    SerialPortScanner.ManualStatusChangeUA();
                    break;
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!reallyClose)
            {
                // Cancel the close and minimize to tray
                e.Cancel = true;
                MinimizeToTray();
            }
            else
            {
                // Actually close
                _settingsService.Save();
                SerialPortScanner.StopComListener();

                if (trayIcon != null)
                {
                    trayIcon.Visible = false;
                    trayIcon.Dispose();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            form1_show_show();
            label7.Visible = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (checkBox1.Checked)
            {
                // Add the value in the registry so that the application runs at startup
                registryKey.SetValue("Busy Light", Application.ExecutablePath);
                _settingsService.Settings.StartWithWindows = true;
                _settingsService.Save();
            }
            else
            {
                registryKey.DeleteValue("Busy Light", false);
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void label3_Click_1(object sender, EventArgs e)
        {

        }
        private void Info_Show()
        {

        }
        private void Info_Click(object sender, EventArgs e)
        {
            form1_show_hide();
            label7.Text = "Busy Light v1.0\n\nDeveloped by Red Den Software\n\nThis application connects to your RingCentral account to monitor your presence status\n\n and updates a connected busy light accordingly. It also allows manual control of the light\n\n and customization of settings.\n\nFor support, please contact support@redden.dev";
            label7.Visible = true;
        }
        
        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}


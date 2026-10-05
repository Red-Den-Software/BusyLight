using RingCentral;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Management;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static rc_program.Program;
using Busy_Light;

namespace Busy_Light
{
    public class main
    {
        public static SerialPort _serialPort;
        public static string port = null;
        public static CancellationTokenSource _cts;
    }
    public class SerialHeartBeatManager : IDisposable
    {
        private SerialPort _serialPort;
        private Form1 form1;
        private readonly object _portLock = new object();
        private CancellationTokenSource _cts;
        private Task _readLoopTask;
        private DateTime _lastHeartbeatTime = DateTime.MinValue;

        public event Action<byte> OnHeartbeatReceived;
        public event Action<bool> OnConnectionStatusChanged;
        public  SerialHeartBeatManager(string port, int baudRate = 9600)
        {
            _serialPort = new SerialPort(port, baudRate, Parity.None, 8, StopBits.One)
            {
                ReadTimeout = 2000,
                WriteTimeout = 2000
            };
            
        }
        public void Start()
        {
            try
            {
                if (!_serialPort.IsOpen)
                {
                    _serialPort.Open();
                    Debug.WriteLine($"Serial port {_serialPort.PortName} opened.");
                    _cts = new CancellationTokenSource();
                    _lastHeartbeatTime = DateTime.Now;

                    _readLoopTask = Task.Run(() => ReadLoop(_cts.Token));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to open serial port {_serialPort.PortName}: {ex.Message}");
                MessageBox.Show($"Failed to open serial port {_serialPort.PortName}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ReadLoop(CancellationToken token)
        {
            while (!token.IsCancellationRequested && _serialPort.IsOpen)
            {
                try
                {
                    if (_serialPort.BytesToRead > 0)
                    {
                        byte data = (byte)_serialPort.ReadByte();
                        
                        ProcessIncomingData(data);
                    }

                    bool isAlive =
                        (DateTime.Now - _lastHeartbeatTime).TotalSeconds < 4;
                   
                    ComPortListener.IsConnected = isAlive;

                    Thread.Sleep(50);
                }
                catch (TimeoutException)
                {
                    // No data received within ReadTimeout.
                    
                    ComPortListener.IsConnected = false;
                }
                catch (Exception ex)
                {
                    if (!token.IsCancellationRequested)
                    {
                        Debug.WriteLine($"Serial error: {ex.Message}");
                    }
                }
            }
        }
        private void ProcessIncomingData(byte data)
        {
            const byte HEARTBEAT_BYTE = 0x3F;
            Debug.WriteLine($"Received byte: 0x{data:X2}");
            // Adjust "HEARTBEAT" to match your device's actual heartbeat protocol string/byte
            if (data == HEARTBEAT_BYTE) // Replace 0x01 with your actual heartbeat byte
            {
                _lastHeartbeatTime = DateTime.Now;
                OnHeartbeatReceived?.Invoke(data);
            }
        }

        public void SendData(byte[] message)
        {
            if (_serialPort == null || !_serialPort.IsOpen)
            {
                MessageBox.Show($"Serial port is not open. Please check the connection and try again. Serial Port: {_serialPort?.PortName ?? "Unknown"}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new InvalidOperationException("Serial port is not open.");
            }

            lock (_portLock)
            {
                _serialPort.Write(message, 0, message.Length);
            }
        }

        public void Dispose()
        {
            _cts?.Cancel();
            _readLoopTask?.Wait(1000);
            _cts?.Dispose();
            if (_serialPort != null && _serialPort.IsOpen)
            {
                _serialPort.Close();
                _serialPort.Dispose();
            }
        }

    }
    public class ComPortListener
    {

        private static SerialPort _serialPort;
        public static SerialHeartBeatManager heartbeatManager;
        public static bool ComPortHardwareIDFinder()
        {
            string query = "SELECT * FROM Win32_SerialPort";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

            foreach (ManagementObject port in searcher.Get())
            {
                string deviceId = port["DeviceID"]?.ToString();
                string pnpDeviceId = port["PNPDeviceID"]?.ToString();
                string description = port["Description"]?.ToString();

                if (pnpDeviceId != null && pnpDeviceId.Contains("VID_303A&PID_1001"))
                {
                    Debug.WriteLine($"Found COM port: {deviceId} - {description}");

                    main.port = deviceId;

                    heartbeatManager = new SerialHeartBeatManager(main.port);
                    heartbeatManager.Start();

                    return true;
                }
            }

            return false;
        }




        public static bool IsConnected;
            
        public static void SendBrightnessToArduino(int value)
        {
            try
            {
                    byte command = 0x03;
                    byte brightness = (byte)value;
                    byte[] data = { command, brightness };
                    ComPortListener.heartbeatManager.SendData(data);
                    Debug.WriteLine($"Sent to Arduino: 0x03, {brightness}");
             }
            catch (Exception ex)
            {
                Debug.WriteLine($"Serial write failed: {ex.Message}");
            }
        }

       
        
        private static void OnTelephonyStatusChanged(string status)
        {
            string[] targetStatuses = { "Ringing", "CallConnected" };
            string[] availableStatuses = { "NoCall", "Disconnected" };
            System.Diagnostics.Debug.WriteLine($"Port open? {_serialPort?.IsOpen}");
            if (targetStatuses.Contains(status))
            {
                try
                {
                   
                    System.Diagnostics.Debug.WriteLine($"Writing 0x02 for status {status}");
                    ComPortListener.heartbeatManager.SendData(new byte[] { 0x02 });

                    System.Diagnostics.Debug.WriteLine($"Telephony Status: {status}");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Write error: {ex.Message}");
                }
            }
            if (availableStatuses.Contains(status))
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine($"Writing 0x01 for status {status}");
                    ComPortListener.heartbeatManager.SendData(new byte[] { 0x01 });

                    System.Diagnostics.Debug.WriteLine($"Telephony Status: {status}");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Write error: {ex.Message}");
                }
            }
        }


    }
    public class  ComPortFunctions
    {
       
        public static async Task CloseComPortSession()
        {
            main._cts?.Cancel();

            if (main._serialPort != null && main._serialPort.IsOpen)
            {
                main._serialPort.Close();
            }
        }
        
        public static void ManualStatusChange(string status)
        {
           
            if (status == "Unavailable")
            {
                byte[] unavailable = { 0x02 };
               
                ComPortListener.heartbeatManager.SendData(unavailable);
              
                MessageBox.Show($"Sent Unavailable to Arduino" , "Status Change", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (status == "Available")
            {
                byte[] available = { 0x01 };
                ComPortListener.heartbeatManager.SendData(available);
            }

            
        }
        public enum ESPStatus
        {
            Available = 0x01,
            Unavailable = 0x02,
            SetBrightness = 0x03
        }
        public static void SendStatusToESP(ESPStatus status, int brightness = 0)
        {
            if (main._serialPort != null && main._serialPort.IsOpen)
            {
                switch (status)
                {
                    case ESPStatus.Available:
                        byte[] available = { 0x01 };
                        ComPortListener.heartbeatManager.SendData(available);
                        System.Diagnostics.Debug.WriteLine("Sent Available to Arduino");
                        break;
                    case ESPStatus.Unavailable:
                        byte[] unavailable = { 0x02 };
                        ComPortListener.heartbeatManager.SendData(unavailable);
                        System.Diagnostics.Debug.WriteLine("Sent Unavailable to Arduino");
                        break;
                    case ESPStatus.SetBrightness:
                        byte command = 0x03;
                        byte brightnessValue = (byte)brightness;
                        ComPortListener.heartbeatManager.SendData(new byte[] { command, brightnessValue });
                        System.Diagnostics.Debug.WriteLine($"Sent SetBrightness to Arduino: {brightnessValue}");
                        break;
                }
            }
        }
    }
}

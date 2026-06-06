using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static rc_program.Program;

namespace Busy_Light
{
    public class SerialPortScanner
    {

        public static SerialPort _serialPort;
        static string port = null;



        //public static void ifport(Form1 form)
        //{
        //    if (IsConnected)
        //        return;
        //    port = FindDevicePort();
        //    if (port != null)
        //    {
        //        StartComListener(port, form);
        //    }
        //    else
        //    {
        //        Debug.WriteLine("Device not found.");

        //        form.textBox1.Text = "Disconnected";
        //        form.textBox1.BackColor = Color.Red;
        //    }
        //}
        private static CancellationTokenSource _cts;

        public static void StartComListener(string port, Form1 form)
        {
            Form1 form1 = form;
            _cts = new CancellationTokenSource();

            Task.Run(async () =>
            {

                while (!_cts.Token.IsCancellationRequested)
                {
                    try
                    {

                        System.Diagnostics.Debug.WriteLine("Attempting to open COM port...");

                        _serialPort = new SerialPort(port, 9600, Parity.None, 8, StopBits.One)
                        {
                            ReadTimeout = 2000,
                            WriteTimeout = 2000
                        };

                        _serialPort.Open();
                        await Task.Delay(2000); // allow port to stabilize

                        if (_serialPort.IsOpen)
                        {
                            System.Diagnostics.Debug.WriteLine($"{port} opened successfully.");
                            byte[] available = { 0x01 };
                            _serialPort.Write(available, 0, 1);
                           form1.UpdateConnectionStatus();

                            // Subscribe once connected
                            PresenceChannel.OnTelephonyStatusChanged += OnTelephonyStatusChanged;

                            break; // EXIT LOOP when connected
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"COM error: {ex.Message}");

                       form1.UpdateConnectionStatus(); break;
                    }

                    // wait before retrying
                    await Task.Delay(3000);
                }
            });
        }
        public static void StopComListener()
        {
            _cts?.Cancel();

            if (_serialPort != null && _serialPort.IsOpen)
            {
                _serialPort.Close();
            }
        }

        public static bool IsConnected =>
            _serialPort != null && _serialPort.IsOpen;
        public static void SendBrightnessToArduino(int value)
        {
            try
            {
                if (_serialPort != null && _serialPort.IsOpen)
                {
                    byte command = 0x03;
                    byte brightness = (byte)value;
                    _serialPort.Write(new byte[] { command, brightness }, 0, 2);
                    _serialPort.BaseStream.Flush();

                    Debug.WriteLine($"Sent to Arduino: 0x03, {brightness}");
                }
                else
                {
                    Debug.WriteLine("Serial port not open!");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Serial write failed: {ex.Message}");
            }
        }
        public static string oncall_color { get; set; }
        public static void ManualStatusChangeUA()
        {

            if (!IsConnected)
            {
                Debug.WriteLine("Cannot send manual status — COM not started.");
                return;
            }
            try
            {

                if (_serialPort != null && _serialPort.IsOpen)
                {
                    byte[] command = { 0x02 };
                    _serialPort.Write(command, 0, 1);
                    //_serialPort.NewLine = command;
                    System.Diagnostics.Debug.WriteLine($"Port open: {_serialPort?.IsOpen}");

                    Debug.WriteLine($"Sent to Arduino: 0x02, {command[0]}");
                }
                else
                {
                    Debug.WriteLine("Serial port not open!");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Serial write failed: {ex.Message}");
            }
        }
        public static void ManualStatusChangeA()
        {
            if (!IsConnected)
            {
                Debug.WriteLine("Cannot send manual status — COM not started.");
                return;
            }
            try
            {
                if (_serialPort != null && _serialPort.IsOpen)
                {
                    byte[] command = { 0x01 };

                    _serialPort.Write(command, 0, 1);
                    _serialPort.BaseStream.Flush();

                    Debug.WriteLine($"Sent to Arduino: 0x01, {command}");
                }
                else
                {
                    Debug.WriteLine("Serial port not open!");
                }
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
            if (_serialPort == null || !_serialPort.IsOpen)
                return;
            System.Diagnostics.Debug.WriteLine($"Port open? {_serialPort?.IsOpen}");
            if (targetStatuses.Contains(status))
            {
                try
                {
                    if (_serialPort == null || !_serialPort.IsOpen)
                    {
                        System.Diagnostics.Debug.WriteLine("Serial port not open!");
                        return;
                    }
                    System.Diagnostics.Debug.WriteLine($"Writing 0x02 for status {status}");

                    byte[] unavailable = { 0x02 };
                    _serialPort.Write(unavailable, 0, 1);

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
                    byte[] available = { 0x01 };
                    _serialPort.Write(available, 0, 1);

                    System.Diagnostics.Debug.WriteLine($"Telephony Status: {status}");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Write error: {ex.Message}");
                }
            }
        }


    }
}

using RingCentral;
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
    public class main
    {
        public static SerialPort _serialPort;
        static string port = null;
        public static CancellationTokenSource _cts;
    }
    public class SerialPortScanner
    {

        public static SerialPort _serialPort;

        public static void StartComListener(string port, Form1 form)
        {
            
            Form1 form1 = form;
            main._cts = new CancellationTokenSource();

            Task.Run(async () =>
            {

                while (!main._cts.Token.IsCancellationRequested)
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
    public class  ComPortFunctions
    {
        
        public async Task StopComListener()
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
                SendStatusToESP(ESPStatus.Unavailable);
            }
            else if (status == "Available")
            {
                SendStatusToESP(ESPStatus.Available);
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
                        main._serialPort.Write(available, 0, 1);
                        System.Diagnostics.Debug.WriteLine("Sent Available to Arduino");
                        break;
                    case ESPStatus.Unavailable:
                        byte[] unavailable = { 0x02 };
                        main._serialPort.Write(unavailable, 0, 1);
                        System.Diagnostics.Debug.WriteLine("Sent Unavailable to Arduino");
                        break;
                    case ESPStatus.SetBrightness:
                        byte command = 0x03;
                        byte brightnessValue = (byte)brightness;
                        main._serialPort.Write(new byte[] { command, brightnessValue }, 0, 2);
                        System.Diagnostics.Debug.WriteLine($"Sent SetBrightness to Arduino: {brightnessValue}");
                        break;
                }
            }
        }
    }
}

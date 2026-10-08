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
    public class SerialPortManager
    {
        public static SerialPort _serialPort;
        public static string port = null;
        public static CancellationTokenSource _cts;
    }
    public class SerialHeartBeatManager : IDisposable
    {
        private SerialPort _serialPort;
        private readonly object _portLock = new object();

        private CancellationTokenSource? _cts;
        private Task? _readLoopTask;

        private DateTime _lastHeartbeatTime = DateTime.MinValue;

        private readonly string _portName;
        private readonly int _baudRate;

        private const int HEARTBEAT_TIMEOUT_SECONDS = 10;
        private const int RECONNECT_DELAY_MS = 2000;

        private System.Threading.Timer? _heartbeatTimer;

        public event Action<byte>? OnHeartbeatReceived;
        public event Action<bool>? OnConnectionStatusChanged;


        public SerialHeartBeatManager(string port, int baudRate = 9600)
        {
            _portName = port;
            _baudRate = baudRate;

            _serialPort = new SerialPort(
                _portName,
                _baudRate,
                Parity.None,
                8,
                StopBits.One)
            {
                ReadTimeout = 2000,
                WriteTimeout = 2000
            };

            _heartbeatTimer = new System.Threading.Timer(
                CheckHeartbeat,
                null,
                1000,
                1000);
        }


        public void Start()
        {
            if (_cts != null && !_cts.IsCancellationRequested)
                return;

            _cts = new CancellationTokenSource();

            _readLoopTask = Task.Run(
                () => ConnectionLoopAsync(_cts.Token));
        }


        private async Task ConnectionLoopAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    // Port is not open - try to open it
                    if (!_serialPort.IsOpen)
                    {
                        TryOpenPort();

                        if (!_serialPort.IsOpen)
                        {
                            await Task.Delay(RECONNECT_DELAY_MS, token);
                            continue;
                        }
                    }

                    // Read data while port is open
                    await ReadLoopAsync(token);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(
                        $"Serial connection error: {ex.Message}");

                    SetDisconnected();

                    ClosePort();

                    try
                    {
                        await Task.Delay(RECONNECT_DELAY_MS, token);
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                }
            }
        }


        private void TryOpenPort()
        {
            lock (_portLock)
            {
                try
                {
                    if (_serialPort.IsOpen)
                        return;

                    // COM port may have disappeared and returned.
                    // Recreate the SerialPort object before opening.
                    _serialPort.Dispose();

                    _serialPort = new SerialPort(
                        _portName,
                        _baudRate,
                        Parity.None,
                        8,
                        StopBits.One)
                    {
                        ReadTimeout = 2000,
                        WriteTimeout = 2000
                    };

                    _serialPort.Open();

                    Debug.WriteLine(
                        $"Serial port {_portName} opened.");

                    // Do NOT mark IsConnected true here.
                    // We need the heartbeat to prove the ESP32 is alive.
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(
                        $"Unable to open {_portName}: {ex.Message}");

                    try
                    {
                        _serialPort.Dispose();
                    }
                    catch
                    {
                    }
                }
            }
        }


        private async Task ReadLoopAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    if (!_serialPort.IsOpen)
                    {
                        SetDisconnected();
                        return;
                    }

                    if (_serialPort.BytesToRead > 0)
                    {
                        int data = _serialPort.ReadByte();

                        if (data >= 0)
                        {
                            Debug.WriteLine(
                                $"RECEIVED: 0x{data:X2}");

                            ProcessIncomingData((byte)data);
                        }
                    }
                    else
                    {
                        await Task.Delay(10, token);
                    }
                }
                catch (OperationCanceledException)
                {
                    return;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(
                        $"SERIAL ERROR: {ex.Message}");

                    SetDisconnected();

                    ClosePort();

                    return;
                }
            }
        }


        private void ProcessIncomingData(byte data)
        {
            const byte HEARTBEAT_BYTE = 0x3F;

            Debug.WriteLine(
                $"Received byte: 0x{data:X2}");

            // Ignore everything except heartbeat
            if (data != HEARTBEAT_BYTE)
                return;

            // Heartbeat received
            _lastHeartbeatTime = DateTime.UtcNow;

            Debug.WriteLine(
                "ESP32 heartbeat detected.");

            // This is the important reconnect detection
            if (!ComPortListener.IsConnected)
            {
                Debug.WriteLine(
                    "ESP32 RECONNECTED - heartbeat detected.");

                ComPortListener.IsConnected = true;

                OnConnectionStatusChanged?.Invoke(true);
            }

            OnHeartbeatReceived?.Invoke(data);
        }


        public void CheckHeartbeat(object? state)
        {
            if (_lastHeartbeatTime == DateTime.MinValue)
                return;

            var elapsed =
                DateTime.UtcNow - _lastHeartbeatTime;

            if (elapsed.TotalSeconds >
                HEARTBEAT_TIMEOUT_SECONDS)
            {
                if (ComPortListener.IsConnected)
                {
                    Debug.WriteLine(
                        $"ESP32 HEARTBEAT LOST. " +
                        $"Last heartbeat: {elapsed.TotalSeconds:F1}s ago.");

                    SetDisconnected();
                }
            }
        }


        private void SetDisconnected()
        {
            if (ComPortListener.IsConnected)
            {
                Debug.WriteLine(
                    "ESP32 is disconnected.");

                ComPortListener.IsConnected = false;

                OnConnectionStatusChanged?.Invoke(false);
            }
        }


        private void ClosePort()
        {
            lock (_portLock)
            {
                try
                {
                    if (_serialPort.IsOpen)
                        _serialPort.Close();
                }
                catch
                {
                }
            }
        }


        public void SendData(byte[] message)
        {
            lock (_portLock)
            {
                if (_serialPort == null ||
                    !_serialPort.IsOpen)
                {
                    throw new InvalidOperationException(
                        $"Serial port {_portName} is not open.");
                }

                _serialPort.Write(
                    message,
                    0,
                    message.Length);
            }
        }


        public void Dispose()
        {
            _heartbeatTimer?.Dispose();
            _heartbeatTimer = null;

            _cts?.Cancel();

            try
            {
                _readLoopTask?.Wait(1000);
            }
            catch
            {
            }

            _cts?.Dispose();
            _cts = null;

            ClosePort();

            try
            {
                _serialPort.Dispose();
            }
            catch
            {
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

                    SerialPortManager.port = deviceId;

                    heartbeatManager = new SerialHeartBeatManager(SerialPortManager.port);
                    heartbeatManager.Start();

                    return true;
                }
            }

            return false;
        }




        public static bool IsConnected;
            
        public static void SendBrightnessToArduino(int value)
        {
            if (heartbeatManager == null)
            {
                Debug.WriteLine("Cannot send brightness: heartbeat manager is not initialized.");
                return;
            }

            try
            {
                byte command = 0x03;
                byte brightness = (byte)Math.Clamp(value, 0, 255);

                byte[] data =
                {
            command,
            brightness
        };

                heartbeatManager.SendData(data);

                Debug.WriteLine(
                    $"Sent to Arduino: 0x03, {brightness}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(
                    $"Serial write failed: {ex.Message}");
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
            SerialPortManager._cts?.Cancel();

            if (SerialPortManager._serialPort != null && SerialPortManager._serialPort.IsOpen)
            {
                SerialPortManager._serialPort.Close();
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
            if (SerialPortManager._serialPort != null && SerialPortManager._serialPort.IsOpen)
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

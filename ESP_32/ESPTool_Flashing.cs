using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Busy_Light.ESP_32
{
    public class ESPTool_Flashing
    {
        public void FlashPath()
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var appFirmwarePath = Path.Combine(appDataPath, "RedDenSoftware", "BusyLight", "Firmware");
            var firmwarePath = Path.Combine(appFirmwarePath, "firmware.bin");
            Directory.CreateDirectory(appFirmwarePath);
        }
       public static void FlashESP32(string port, string firmwarePath)
        {
            // Construct the command to flash the ESP32
            string command = $"esptool.exe --chip esp32 --port {port} --baud 9600 write_flash -z 0x1000 {firmwarePath}";
            // Start the process to execute the command
            var processInfo = new System.Diagnostics.ProcessStartInfo("cmd.exe", "/c " + command)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            using (var process = new System.Diagnostics.Process())
            {
                process.StartInfo = processInfo;
                process.OutputDataReceived += (sender, args) => Console.WriteLine(args.Data);
                process.ErrorDataReceived += (sender, args) => Console.WriteLine("ERROR: " + args.Data);
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();
            }
        }

    }
}

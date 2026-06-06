using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Busy_Light.Form1;

namespace Busy_Light
{
    public class UserSettings
    {
        public string Theme { get; set; }
        public int Brightness { get; set; }
        public bool StartWithWindows { get; set; }
        public string oncall_color { get; set; }
        public string offcall_color { get; set; }
        public string msgrecieved_color { get; set; }
    }
    public class ServiceSettings
    {
        private readonly string _filePath;
            public UserSettings Settings { get; private set; }

            public ServiceSettings()
            {
                var appDataPath = Environment.GetFolderPath(
            Environment.SpecialFolder.ApplicationData); // Roaming

                var appFolder = Path.Combine(appDataPath, "RedDenSoftware", "BusyLight");

                Directory.CreateDirectory(appFolder); // Ensure folder exists

                _filePath = Path.Combine(appFolder, "usersettings.json");
                Load();
            }

        public void Load()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                Settings = JsonSerializer.Deserialize<UserSettings>(json);
            }
            else
            {
                Settings = new UserSettings
                {
                    Theme = "Dark",
                    Brightness = 255,
                    StartWithWindows = false,
                    oncall_color = "Red",
                    offcall_color = "Green",
                    msgrecieved_color = "Orange"
                };
                Save(); // create file first time
            }
        }

        public void Save()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(Settings, options);
            File.WriteAllText(_filePath, json);
        }
    }
}

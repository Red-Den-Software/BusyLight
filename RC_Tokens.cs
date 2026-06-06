using RingCentral;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Busy_Light
{
    public class TokenService
    {
        private readonly string _tokenPath;
        
        public TokenService()
        {
            var folder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "RedDenSoftware",
                "BusyLight");

            Directory.CreateDirectory(folder);
            _tokenPath = Path.Combine(folder, "rctoken.json");
        }

        public void Save(TokenInfo token)
        {
           
            Directory.CreateDirectory(
                Path.GetDirectoryName(_tokenPath)!);

            string json = JsonSerializer.Serialize(token);

            File.WriteAllText(_tokenPath, json);
        }
        private JsonElement _refreshToken;

        public static TokenInfo refresh_token { get; internal set; }

        public class tokens
        {
            [JsonPropertyName("access_token")]
            public string access_token { get; set; }
            public int expires_in { get; set; }
            [JsonPropertyName("refresh_token")]
            public string refresh_token { get; set; }
            
        }
        public TokenInfo? Load()
        {
            if (!File.Exists(_tokenPath))
                return null;

            string json = File.ReadAllText(_tokenPath);

            if (string.IsNullOrWhiteSpace(json))
                return null;

            return JsonSerializer.Deserialize<TokenInfo>(json);
        }

        public bool Clear()
        {
            if (File.Exists(_tokenPath))
                File.Delete(_tokenPath);
            return true;
        }
    }
}

using Busy_Light;
using Busy_Light.RC;
using Newtonsoft.Json.Linq;
using RingCentral;
using RingCentral.Net;
using RingCentral.Net.WebSocket;
using System.Data;
using System.IO.Ports;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Channels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Timers;
using static Busy_Light.SerialPortScanner;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Timer = System.Timers.Timer;
using static Busy_Light.Form1;
namespace rc_program

{

    public class Program
    {
        
        public List<Presence.Body> presenceData { get; set; }
        Presence.Body presence;
        public static class PresenceChannel
        {
            
            public static string TelephonyStatus { get; private set; }
            public static event Action<string> OnTelephonyStatusChanged;
            public static void Update(string telephonyStatus)
            {
                TelephonyStatus = telephonyStatus;
                OnTelephonyStatusChanged?.Invoke(telephonyStatus);
            }
        }
        public static string TelephonyStatus { get; set; }
        public static async Task Status(string message)
        {
            var j = JToken.Parse(message);

            JToken bodyToken = j["body"] ?? j;

            System.Diagnostics.Debug.WriteLine($"MEssage here {message}");
            List<Presence.Body> presenceList;

            if (bodyToken.Type == JTokenType.Array)
            {
                presenceList = bodyToken.ToObject<List<Presence.Body>>();
            }
            else if (bodyToken.Type == JTokenType.Object)
            {
                var single = bodyToken.ToObject<Presence.Body>();
                presenceList = single != null
                    ? new List<Presence.Body> { single }
                    : new List<Presence.Body>();
            }
            else
            {
                presenceList = new List<Presence.Body>();
            }

            foreach (var p in presenceList)
            {
                System.Diagnostics.Debug.WriteLine($"Status: {p?.telephonyStatus}");
            }

            if (presenceList.Count > 0 && !string.IsNullOrWhiteSpace(presenceList[0]?.telephonyStatus))
            {
                TelephonyStatus = presenceList[0].telephonyStatus;

                PresenceChannel.Update(TelephonyStatus);

                System.Diagnostics.Debug.WriteLine($"Parsed Telephony Status: {TelephonyStatus}");
            }

            await Task.CompletedTask;
        }
        static RestClient _restClient;


        
        
    }

    }

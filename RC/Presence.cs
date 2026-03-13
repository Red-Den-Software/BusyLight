using System.Text.Json;
using RingCentral;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RingCentral.Net;
using Newtonsoft.Json.Linq;
namespace Busy_Light.RC
{

    public class Presence
    {
       

        public class Body
        {
            public string? extensionId { get; set; }
            public  string? telephonyStatus { get; set; }

            public int sequence { get; set; }

            public string? presenceStatusmeetingStatus { get; set; }

            public string? userStatus { get; set; }

            public string? meetingStatus { get; set; }

            public string? dndStatus { get; set; }
            public bool allowSeeMyPresence { get; set; }
            public bool ringOnMonitoredCall { get; set; }
            public bool pickUpCallsOnHold { get; set; }

        }
     
        }
    }

            
        
    

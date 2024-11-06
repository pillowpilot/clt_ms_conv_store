using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Schemas.Tickets
{
    public class EventDetails
    {
        public string Type { get; set; }
        public string Version { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
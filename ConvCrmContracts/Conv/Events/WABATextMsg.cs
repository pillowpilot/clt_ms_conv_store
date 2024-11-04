using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConvCrmContracts.Conv.Events
{
    public class WABATextMsg : IBaseEvent
    {
        public string type { get; set; } = default!;

        public string uuid { get; set; } = default!;

        public DateTime timestamp { get; set; }

        public string sender { get; set; } = default!;

        public string receiver { get; set; } = default!;

        public string body { get; set; } = default!;

        public string source_id { get; set; } = default!;

        public string message_id { get; set; } = default!;

    }
}
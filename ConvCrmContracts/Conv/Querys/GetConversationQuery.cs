using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConvCrmContracts.Conv.Querys
{
    public class GetConversationQuery 
    {
        public required Guid Uuid { get; set; }
        public DateTime Timestamp { get; set; }
        public string source_id { get; set; }
        public DateTime? start_timestamp { get; set; }
        public DateTime? end_timestamp { get; set; }
    }
}
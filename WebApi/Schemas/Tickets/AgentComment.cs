using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Schemas.Tickets
{
    public class AgentComment
    {
        [BsonElement("timestamp")]
        public DateTime Timestamp { get; set; }

        [BsonElement("body")]
        public string Body { get; set; }

        [BsonElement("agent_id")]
        public string AgentId { get; set; }

        [BsonElement("agent_name")]
        public string AgentName { get; set; }
    }
}
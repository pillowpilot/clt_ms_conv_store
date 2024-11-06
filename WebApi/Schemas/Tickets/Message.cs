using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Schemas.Tickets
{
    public class Message()
    {

        [BsonElement("timestamp")]
        public DateTime Timestamp { get; set; }

        [BsonElement("body")]
        public string Body { get; set; }

        [BsonElement("origin")]
        public string Origin { get; set; }

        [BsonElement("origin_details")]
        public OriginDetails OriginDetails { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Schemas.Tickets
{
    public class LastMessage
    {
        [BsonElement("timestamp")]
        public string Timestamp { get; set; }

        [BsonElement("body")]
        public string Body { get; set; }
    }
}
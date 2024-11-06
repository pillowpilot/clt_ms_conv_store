using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Schemas.Tickets
{
    public class OriginDetails
    {
        [BsonElement("display_name")]
        public string DisplayName { get; set; }
    }
}
using Newtonsoft.Json;

namespace WebApi.Integration.Common;

public class ChannelDetails
{
    public int id { get; set; }

    public string phone_number { get; set; } = default!;
}

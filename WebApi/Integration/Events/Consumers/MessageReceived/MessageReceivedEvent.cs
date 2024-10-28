using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WebApi.Integration.Events.Consumers.MessageReceived;

public class MessageReceivedEvent : BaseEvent
{
    public string sender { get; set; } = default!;

    public string receiver { get; set; } = default!;

    public string body { get; set; } = default!;

    public string source_id { get; set; } = default!;

    public string channel { get; set; } = default!;

    public ChannelDetails? channel_details { get; set; }

    public UserDetails? user_details { get; set; }
}
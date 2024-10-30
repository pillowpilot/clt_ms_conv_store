namespace ConvCrmContracts.Conv.Events;

public class MessageReceivedEvent : IBaseEvent
{
    public string type { get; set; } = default!;

    public string uuid { get; set; } = default!;

    public DateTime timestamp { get; set; }

    public string sender { get; set; } = default!;

    public string receiver { get; set; } = default!;

    public string body { get; set; } = default!;

    public string source_id { get; set; } = default!;

    public string channel { get; set; } = default!;

    public ChannelDetails? channel_details { get; set; }

    public UserDetails? user_details { get; set; }
}
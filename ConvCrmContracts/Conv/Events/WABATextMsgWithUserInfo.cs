namespace ConvCrmContracts.Conv.Events;

public class WABATextMsgWithUserInfo : IBaseEvent
{
    public Guid uuid { get; set; }

    public DateTime timestamp { get; set; }

    public string sender { get; set; } = default!;

    public string receiver { get; set; } = default!;

    public string body { get; set; } = default!;

    public ChannelDetails? channel_details { get; set; }

    public UserDetails? user_details { get; set; }
}

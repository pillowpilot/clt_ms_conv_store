namespace ConvCrmContracts.Conv.Commands;

public class OpenTicket : IBaseEvent
{
    public string type { get; set; } = default!;
    public string uuid { get; set; } = default!;
    public DateTime timestamp { get; set; }
    public Guid conversation_id { get; set; }
    public string source_id { get; set; } = default!;
    public string channel { get; set; } = default!;
    public UserDetails? user_details { get; set; }

    public static OpenTicket Create(Guid conversationId, string sourceId, string channel, UserDetails? userDetails)
    {
        return new OpenTicket
        {
            type = "v0.conv.open_ticket",
            uuid = Guid.NewGuid().ToString(),
            timestamp = DateTime.Now,
            conversation_id = conversationId,
            source_id = sourceId,
            channel = channel,
            user_details = userDetails
        };
    }
}

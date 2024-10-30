namespace ConvCrmContracts.Conv.Commands;

public class OpenTicketCommand : IBaseEvent
{
    public string type { get; set; } = default!;
    public string uuid { get; set; } = default!;
    public DateTime timestamp { get; set; }
    public Guid? conversation_id { get; set; }
    public UserDetails? user_details { get; set; }

    public static OpenTicketCommand Create(Guid conversationId, UserDetails? userDetails)
    {
        return new OpenTicketCommand
        {
            type = "v0.conv.open_ticket",
            uuid = Guid.NewGuid().ToString(),
            timestamp = DateTime.Now,
            conversation_id = conversationId,
            user_details = userDetails
        };
    }
}

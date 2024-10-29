namespace WebApi.Integration.Events.Consumers.TicketStateChanged;

public class TicketStateChangedEvent : IBaseEvent
{
    public string type { get; set; } = default!;
    public string uuid { get; set; } = default!;
    public DateTime timestamp { get; set; }
    public string ticket_id { get; set; } = default!;
    public string new_state { get; set; } = default!;
    public AuthorizedBy authorized_by { get; set; } = new();
}

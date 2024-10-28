namespace WebApi.Integration.Events.Consumers.TicketStateChanged;

public class TicketStateChangedEvent : BaseEvent
{
    public string ticket_id { get; set; } = default!;
    public string new_state { get; set; } = default!;
    public AuthorizedBy authorized_by { get; set; } = new();
}

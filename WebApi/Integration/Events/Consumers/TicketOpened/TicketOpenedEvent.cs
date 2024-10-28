namespace WebApi.Integration.Events.Consumers.TicketOpened;

public class TicketOpenedEvent : BaseEvent
{
    public string ticket_id { get; set; } = default!;
    public string conversation_id { get; set; } = default!;
}

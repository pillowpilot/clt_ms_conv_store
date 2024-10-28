namespace WebApi.Integration.Events.Consumers.TicketCommentAdded;

public class TicketCommentAddedEvent : BaseEvent
{
    public string ticket_id { get; set; } = default!;
    public string comment { get; set; } = default!;
    public AuthorizedBy authorized_by { get; set; } = new();
}

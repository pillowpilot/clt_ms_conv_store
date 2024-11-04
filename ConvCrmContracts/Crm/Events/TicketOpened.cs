namespace ConvCrmContracts.Crm.Events;

public class TicketOpened : IBaseEvent
{
    public Guid uuid { get; set; }
    public DateTime timestamp { get; set; }
    public long ticket_number { get; set; }
    public Guid conversation_id { get; set; }
    public AuthorizedBy authorized_by { get; set; }
}

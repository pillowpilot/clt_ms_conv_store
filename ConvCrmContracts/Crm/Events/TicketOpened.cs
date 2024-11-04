namespace ConvCrmContracts.Crm.Events;

public class TicketOpened : IBaseEvent
{
    public string type { get; set; } = default!;
    public string uuid { get; set; } = default!;
    public DateTime timestamp { get; set; }
    public string ticket_id { get; set; } = default!;
    public string conversation_id { get; set; } = default!;
}

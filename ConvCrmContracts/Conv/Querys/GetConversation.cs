namespace ConvCrmContracts.Conv.Querys;

public class GetConversation : IBaseEvent
{
    public Guid uuid { get; set; }
    public DateTime timestamp { get; set; }
    public int ticket_id { get; set; }
    public Guid conversation_id { get; set; }
    public DateTime? start_timestamp { get; set; }
    public DateTime? end_timestamp { get; set; }
}
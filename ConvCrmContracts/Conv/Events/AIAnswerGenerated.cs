namespace ConvCrmContracts.Conv.Events;

public class AIAnswerGenerated : IBaseEvent
{
    public Guid uuid { get; set; }
    public DateTime timestamp { get; set; }
    public Guid conversation_id { get; set; }
    public string body { get; set; } = default!;
    public bool open_ticket { get; set; } = false;
}
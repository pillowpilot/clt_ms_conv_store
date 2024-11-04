namespace ConvCrmContracts.Crm.Events;

public class ConversationDocumentEvent : IBaseEvent
{
    public string type { get; set; } = default!;
    public string uuid { get; set; } = default!;
    public DateTime timestamp { get; set; }
    public Conversation conversation { get; set; } = new();
}
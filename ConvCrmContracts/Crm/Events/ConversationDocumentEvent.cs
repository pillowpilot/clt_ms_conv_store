namespace ConvCrmContracts.Crm.Events;

public class ConversationDocumentEvent : IBaseEvent
{
    public string type { get; set; } = default!;
    public Guid uuid { get; set; }
    public DateTime timestamp { get; set; }
    //public Conversation conversation { get; set; } = new();
}
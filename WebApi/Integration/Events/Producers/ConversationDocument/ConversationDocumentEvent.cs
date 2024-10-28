using WebApi.Integration.Common;

namespace WebApi.Integration.Events.Producers.ConversationDocument;

public class ConversationDocumentEvent : BaseEvent
{
    public Conversation conversation { get; set; } = new();
}

public class Conversation
{
    public string conversation_id { get; set; } = default!;
    public List<object> messages { get; set; } = [];
}

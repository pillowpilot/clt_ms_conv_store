namespace WebApi.Integration.Events.Producers.ConversationDocument;

public class ConversationDocumentEventProducer : IConsumer<ConversationDocumentEvent>
{
    public Task Consume(ConsumeContext<ConversationDocumentEvent> context)
    {
        throw new NotImplementedException();
    }
}

namespace WebApi.Integration.Events.Consumers.WhatsAppMessageDelivered;

public class WhatsAppMessageDeliveredEventConsumer : IConsumer<WhatsAppMessageDeliveredEvent>
{
    public Task Consume(ConsumeContext<WhatsAppMessageDeliveredEvent> context)
    {
        throw new NotImplementedException();
    }
}

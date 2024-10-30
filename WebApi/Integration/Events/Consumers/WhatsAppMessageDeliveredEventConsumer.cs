namespace WebApi.Integration.Events.Consumers;

public class WhatsAppMessageDeliveredEventConsumer : IConsumer<WhatsAppMessageDeliveredEvent>
{
    public Task Consume(ConsumeContext<WhatsAppMessageDeliveredEvent> context)
    {
        throw new NotImplementedException();
    }
}

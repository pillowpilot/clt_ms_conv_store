namespace WebApi.Integration.Events.Consumers;

public class WhatsAppMessageDeliveredEventConsumer : IConsumer<WhatsAppMessageDelivered>
{
    public Task Consume(ConsumeContext<WhatsAppMessageDelivered> context)
    {
        throw new NotImplementedException();
    }
}

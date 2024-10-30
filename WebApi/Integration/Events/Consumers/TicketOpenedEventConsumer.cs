namespace WebApi.Integration.Events.Consumers;

public class TicketOpenedEventConsumer : IConsumer<TicketOpenedEvent>
{
    public Task Consume(ConsumeContext<TicketOpenedEvent> context)
    {
        throw new NotImplementedException();
    }
}

namespace WebApi.Integration.Events.Consumers;

public class TicketClosedEventConsumer : IConsumer<TicketClosedEvent>
{
    public Task Consume(ConsumeContext<TicketClosedEvent> context)
    {
        throw new NotImplementedException();
    }
}

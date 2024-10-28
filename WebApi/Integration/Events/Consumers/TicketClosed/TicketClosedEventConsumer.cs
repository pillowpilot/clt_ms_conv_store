namespace WebApi.Integration.Events.Consumers.TicketClosed;

public class TicketClosedEventConsumer : IConsumer<TicketClosedEvent>
{
    public Task Consume(ConsumeContext<TicketClosedEvent> context)
    {
        throw new NotImplementedException();
    }
}

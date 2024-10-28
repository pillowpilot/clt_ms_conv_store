namespace WebApi.Integration.Events.Consumers.TicketOpened;

public class TicketOpenedEventConsumer : IConsumer<TicketOpenedEvent>
{
    public Task Consume(ConsumeContext<TicketOpenedEvent> context)
    {
        throw new NotImplementedException();
    }
}

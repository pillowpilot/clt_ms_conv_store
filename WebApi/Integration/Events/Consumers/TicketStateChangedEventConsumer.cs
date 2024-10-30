namespace WebApi.Integration.Events.Consumers;

public class TicketStateChangedEventConsumer : IConsumer<TicketStateChangedEvent>
{
    public Task Consume(ConsumeContext<TicketStateChangedEvent> context)
    {
        throw new NotImplementedException();
    }
}

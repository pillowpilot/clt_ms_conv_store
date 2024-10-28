namespace WebApi.Integration.Events.Consumers.TicketStateChanged;

public class TicketStateChangedEventConsumer : IConsumer<TicketStateChangedEvent>
{
    public Task Consume(ConsumeContext<TicketStateChangedEvent> context)
    {
        throw new NotImplementedException();
    }
}

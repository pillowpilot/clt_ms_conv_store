namespace WebApi.Integration.Events.Consumers;

public class TicketStateChangedEventConsumer : IConsumer<TicketStateChanged>
{
    public Task Consume(ConsumeContext<TicketStateChanged> context)
    {
        throw new NotImplementedException();
    }
}

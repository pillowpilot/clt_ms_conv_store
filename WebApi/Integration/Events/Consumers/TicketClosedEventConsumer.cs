namespace WebApi.Integration.Events.Consumers;

public class TicketClosedEventConsumer : IConsumer<TicketClosed>
{
    public Task Consume(ConsumeContext<TicketClosed> context)
    {
        throw new NotImplementedException();
    }
}

namespace WebApi.Integration.Events.Consumers;

public class TicketOpenedEventConsumer : IConsumer<TicketOpened>
{
    public Task Consume(ConsumeContext<TicketOpened> context)
    {
        throw new NotImplementedException();
    }
}

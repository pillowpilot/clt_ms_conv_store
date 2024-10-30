namespace WebApi.Integration.Events.Consumers;

public class TicketCommentAddedEventConsumer : IConsumer<TicketCommentAddedEvent>
{
    public Task Consume(ConsumeContext<TicketCommentAddedEvent> context)
    {
        throw new NotImplementedException();
    }
}

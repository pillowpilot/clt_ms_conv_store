namespace WebApi.Integration.Events.Consumers;

public class TicketCommentAddedEventConsumer : IConsumer<TicketCommentAdded>
{
    public Task Consume(ConsumeContext<TicketCommentAdded> context)
    {
        throw new NotImplementedException();
    }
}

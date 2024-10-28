namespace WebApi.Integration.Events.Consumers.AgentAnswerGenerated;

public class AgentAnswerGeneratedEventConsumer : IConsumer<AgentAnswerGeneratedEvent>
{
    public Task Consume(ConsumeContext<AgentAnswerGeneratedEvent> context)
    {
        throw new NotImplementedException();
    }
}

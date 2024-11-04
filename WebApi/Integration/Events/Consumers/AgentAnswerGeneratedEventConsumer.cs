namespace WebApi.Integration.Events.Consumers;

public class AgentAnswerGeneratedEventConsumer : IConsumer<AgentAnswerGenerated>
{
    public Task Consume(ConsumeContext<AgentAnswerGenerated> context)
    {
        throw new NotImplementedException();
    }
}

namespace WebApi.Integration.Events.Consumers;

public class AgentAssignedEventConsumer : IConsumer<AgentAssignedEvent>
{
    public Task Consume(ConsumeContext<AgentAssignedEvent> context)
    {
        throw new NotImplementedException();
    }
}

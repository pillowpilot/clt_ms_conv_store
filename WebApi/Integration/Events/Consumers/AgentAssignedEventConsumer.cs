namespace WebApi.Integration.Events.Consumers;

public class AgentAssignedEventConsumer : IConsumer<AgentAssigned>
{
    public Task Consume(ConsumeContext<AgentAssigned> context)
    {
        throw new NotImplementedException();
    }
}

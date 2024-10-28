namespace WebApi.Integration.Events.Consumers.AgentAnswerGenerated;

public class AgentAnswerGeneratedEvent : BaseEvent
{
    public string agent_id { get; set; } = default!;
    public AgentDetails agent_details { get; set; } = new();
    public string conversation_id { get; set; } = default!;
    public string body { get; set; } = default!;
}

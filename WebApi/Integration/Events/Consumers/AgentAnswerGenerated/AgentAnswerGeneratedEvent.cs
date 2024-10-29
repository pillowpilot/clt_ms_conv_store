namespace WebApi.Integration.Events.Consumers.AgentAnswerGenerated;

public class AgentAnswerGeneratedEvent : IBaseEvent
{
    public string type { get; set; } = default!;
    public string uuid { get; set; } = default!;
    public DateTime timestamp { get; set; }
    public string agent_id { get; set; } = default!;
    public AgentDetails agent_details { get; set; } = new();
    public string conversation_id { get; set; } = default!;
    public string body { get; set; } = default!;
}

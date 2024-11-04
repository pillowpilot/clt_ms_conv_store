namespace ConvCrmContracts.Crm.Events;

public class AgentAnswerGenerated : IBaseEvent
{
    public Guid uuid { get; set; }
    public DateTime timestamp { get; set; }
    public string agent_id { get; set; } = default!;
    public AgentDetails agent_details { get; set; }
    public Guid conversation_id { get; set; }
    public string body { get; set; } = default!;
}

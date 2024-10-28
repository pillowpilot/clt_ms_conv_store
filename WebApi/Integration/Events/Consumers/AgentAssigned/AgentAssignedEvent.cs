namespace WebApi.Integration.Events.Consumers.AgentAssigned;

public class AgentAssignedEvent : BaseEvent
{
    public string ticket_id { get; set; } = default!;
    public AuthorizedBy agent_assigned { get; set; } = new();
    public AuthorizedBy authorized_by { get; set; } = new();
}

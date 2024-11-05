namespace WebApi.Schemas.Logs;

public class AssignmentLog(AgentSender newAgent, AgentSender authorizedBy, DateTime timestamp) : Log(Guid.NewGuid(), timestamp)
{
    public AgentSender new_agent { get; private set; } = newAgent;
    public AgentSender authorized_by { get; private set; } = authorizedBy;
}
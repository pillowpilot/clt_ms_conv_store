namespace WebApi.Schemas.Logs;

public class TicketStateLog(AgentSender authorizedBy, string newState = "pending") : Log("state")
{
    public string new_state { get; set; } = newState;
    public AgentSender authorized_by { get; private set; } = authorizedBy;
}



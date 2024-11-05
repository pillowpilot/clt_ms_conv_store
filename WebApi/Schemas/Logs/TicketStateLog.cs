namespace WebApi.Schemas.Logs;

public class TicketStateLog(int ticketId, AgentSender authorizedBy, DateTime timestamp, string newState = "pending") : Log(Guid.NewGuid(), timestamp)
{
    public int ticket_id { get; set; } = ticketId;
    public string new_state { get; set; } = newState;
    public AgentSender authorized_by { get; private set; } = authorizedBy;
}



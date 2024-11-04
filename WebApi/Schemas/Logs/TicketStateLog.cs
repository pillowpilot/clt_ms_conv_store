namespace WebApi.Schemas.Logs;

public class TicketStateLog(long ticketNumber, AgentSender authorizedBy, string newState = "pending") : Log("state")
{
    public long ticket_number { get; set; } = ticketNumber;
    public string new_state { get; set; } = newState;
    public AgentSender authorized_by { get; private set; } = authorizedBy;
}



namespace WebApi.Schemas.Logs;

public class AgentTextLog(string fallbackText, AgentSender sender, DateTime timestamp) : Log(Guid.NewGuid(), timestamp)
{
    public string fallback_text { get; private set; } = fallbackText;
    public AgentSender sender { get; private set; } = sender;
}

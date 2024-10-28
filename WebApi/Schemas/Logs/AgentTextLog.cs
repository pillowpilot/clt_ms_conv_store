namespace WebApi.Schemas.Logs;

public class AgentTextLog(string fallbackText, AgentSender sender) : Log("chat.agent.text")
{
    public string fallback_text { get; private set; } = fallbackText;
    public AgentSender sender { get; private set; } = sender;
}
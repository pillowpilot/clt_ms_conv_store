namespace WebApi.Schemas.Logs;

public class CommentLog(string comment, AgentSender sender, DateTime timestamp) : Log("comment", timestamp)
{
    public string comment { get; private set; } = comment;
    public AgentSender sender { get; private set; } = sender;
}
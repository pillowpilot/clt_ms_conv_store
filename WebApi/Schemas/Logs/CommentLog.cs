namespace WebApi.Schemas.Logs;

public class CommentLog(string comment, AgentSender sender) : Log("comment")
{
    public string comment { get; private set; } = comment;
    public AgentSender sender { get; private set; } = sender;
}
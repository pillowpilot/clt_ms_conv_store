public class Conversation
{
    public string uuid { get; set; } = default!;
    public string message { get; set; }

    public Conversation(Guid id, string message)
    {
        uuid = id.ToString();
        this.message = message;
    }
}

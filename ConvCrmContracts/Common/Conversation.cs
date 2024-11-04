public class Conversation
{
    public Guid uuid { get; set; }
    public string message { get; set; }

    public Conversation(Guid id, string message)
    {
        uuid = id;
        this.message = message;
    }
}

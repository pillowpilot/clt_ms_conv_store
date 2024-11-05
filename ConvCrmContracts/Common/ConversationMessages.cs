namespace ConvCrmContracts.Common;

public class ConversationMessages
{
    public Guid message_id { get; set; }
    public string? id { get; set; }
    public string name { get; set; }
    public string text { get; set; }
    public DateTime time { get; set; }
    public bool is_agent { get; set; }

    public ConversationMessages(Guid message_id, string? id, string name, string text, DateTime time, bool is_agent = true)
    {
        this.message_id = message_id;
        this.id = id;
        this.name = name;
        this.text = text;
        this.time = time;
        this.is_agent = is_agent;
    }
}
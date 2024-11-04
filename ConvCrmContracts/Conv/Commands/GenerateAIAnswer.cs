
namespace ConvCrmContracts.Conv.Commands;

public class GenerateAIAnswer : IBaseEvent
{
    public string type { get; set; } = default!;
    public string uuid { get; set; } = default!;
    public DateTime timestamp { get; set; }
    public Conversation conversation { get; set; }

    public GenerateAIAnswer(string uuid, DateTime timestamp, Conversation conversation)
    {
        this.uuid = uuid;
        this.timestamp = timestamp;
        this.conversation = conversation;
    }
}
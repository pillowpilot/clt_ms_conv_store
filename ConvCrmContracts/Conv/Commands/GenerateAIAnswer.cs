
namespace ConvCrmContracts.Conv.Commands;

public class GenerateAIAnswer : IBaseEvent
{
    public Guid uuid { get; set; }
    public DateTime timestamp { get; set; }
    public Conversation conversation { get; set; }

    public GenerateAIAnswer(Guid uuid, DateTime timestamp, Conversation conversation)
    {
        this.uuid = uuid;;
        this.timestamp = timestamp;
        this.conversation = conversation;
    }
}
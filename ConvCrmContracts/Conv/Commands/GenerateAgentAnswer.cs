
namespace ConvCrmContracts.Conv.Commands;

public class GenerateAgentAnswer : IBaseEvent
{
    public Guid uuid { get; set; }
    public DateTime timestamp { get; set; }
    public Conversation conversation { get; set; }

    public GenerateAgentAnswer(Guid uuid, DateTime timestamp, Conversation conversation)
    {
        this.uuid = uuid;
        this.timestamp = timestamp;
        this.conversation = conversation;
    }
}


namespace ConvCrmContracts.Conv.Commands;

public class GenerateAIAnswerWithUserInfo : IBaseEvent
{
    public string type { get; set; } = default!;
    public string uuid { get; set; } = default!;
    public DateTime timestamp { get; set; }
    public UserDetails? user_details { get; set; }
    public Conversation conversation { get; set; }

    public GenerateAIAnswerWithUserInfo(Guid uuid, DateTime timestamp, UserDetails? user_details, Conversation conversation)
    {
        this.uuid = uuid.ToString();
        this.timestamp = timestamp;
        this.user_details = user_details;
        this.conversation = conversation;
    }
}

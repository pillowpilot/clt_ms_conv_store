using WebApi.Integration.Common;

namespace WebApi.Integration.Queries.GetConversation;

public class GetConversationQuery : IBaseEvent
{
    public string type { get; set; } = default!;
    public string uuid { get; set; } = default!;
    public DateTime timestamp { get; set; }
    public string conversation_id { get; set; } = default!;
}

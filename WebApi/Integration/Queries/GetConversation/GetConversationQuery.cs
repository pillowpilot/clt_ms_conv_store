using WebApi.Integration.Common;

namespace WebApi.Integration.Queries.GetConversation;

public class GetConversationQuery : BaseEvent
{
    public string conversation_id { get; set; } = default!;
}

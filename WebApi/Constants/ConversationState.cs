using System.ComponentModel;

namespace WebApi.Constants;

public enum ConversationState
{
    [Description("open")]
    Open,
    [Description("closed")]
    Closed
}

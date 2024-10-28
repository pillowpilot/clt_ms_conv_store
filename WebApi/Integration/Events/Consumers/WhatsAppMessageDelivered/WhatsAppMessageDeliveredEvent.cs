namespace WebApi.Integration.Events.Consumers.WhatsAppMessageDelivered;

public class WhatsAppMessageDeliveredEvent : BaseEvent
{
    public string sender { get; set; } = default!;
    public string receiver { get; set; } = default!;
    public string message_id { get; set; } = default!;
}

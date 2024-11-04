namespace ConvCrmContracts.Crm.Events;

public class WhatsAppMessageDelivered : IBaseEvent
{
    public string type { get; set; } = default!;
    public string uuid { get; set; } = default!;
    public DateTime timestamp { get; set; }
    public string sender { get; set; } = default!;
    public string receiver { get; set; } = default!;
    public string message_id { get; set; } = default!;
}

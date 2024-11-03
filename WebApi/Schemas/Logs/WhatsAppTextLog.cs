namespace WebApi.Schemas.Logs;

public class WhatsAppTextLog(string fallbackText, WhatsAppSender sender, DateTime timestamp) : Log("chat.wa.text", timestamp)
{
    public string fallback_text { get; private set; } = fallbackText;
    public WhatsAppSender sender { get; private set; } = sender;
}


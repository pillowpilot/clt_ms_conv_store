namespace WebApi.Schemas;

public class WhatsAppSender(string? displayName, string phoneNumber)
{
    public string? display_name { get; set; } = displayName;
    public string phone_number { get; set; } = phoneNumber;
}

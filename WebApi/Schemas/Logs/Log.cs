namespace WebApi.Schemas.Logs;

public class Log(string type, DateTime? timestamp = null)
{
    public string type { get; set; } = $"v0.0.1.{type}";
    public DateTime timestamp { get; set; } = timestamp ?? DateTime.Now;
}
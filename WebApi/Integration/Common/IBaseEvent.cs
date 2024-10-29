namespace WebApi.Integration.Common;

public interface IBaseEvent
{
    public string type { get; set; }
    public string uuid { get; set; }
    public DateTime timestamp { get; set; }
}

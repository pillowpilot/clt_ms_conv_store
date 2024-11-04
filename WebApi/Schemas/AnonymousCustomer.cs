using WebApi.Enums;

namespace WebApi.Schemas;

public class AnonymousCustomer
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid id { get; private set; }

    [BsonRepresentation(BsonType.String)]
    public Channel active_channel { get; private set; }
    public string source_id { get; private set; }

    [BsonRepresentation(BsonType.String)]
    public ManageBy manage_by { get; private set; } = ManageBy.AIAgent;
    public List<Log> logs { get; set; } = [];

    public AnonymousCustomer(string sourceId, Log log, Channel activeChannel = Channel.WhatsApp)
    {
        id = Guid.NewGuid();
        active_channel = activeChannel;
        source_id = sourceId;
        logs.Add(log);
    }
}
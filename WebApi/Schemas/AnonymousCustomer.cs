using WebApi.Enums;

namespace WebApi.Schemas;

public class AnonymousCustomer
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid id { get; private set; }
    public string source_id { get; private set; }

    [BsonRepresentation(BsonType.String)]
    public ManageBy manage_by { get; private set; } = ManageBy.AIAgent;
    public List<Log> logs { get; set; } = [];

    public AnonymousCustomer(string sourceId, Log log)
    {
        id = Guid.NewGuid();
        source_id = sourceId;
        logs.Add(log);
    }

    public AnonymousCustomer(string activeChannel, string sourceId, Log log)
    {
        id = Guid.NewGuid();
        source_id = sourceId;
        logs.Add(log);
    }
}
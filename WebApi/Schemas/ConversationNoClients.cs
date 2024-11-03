namespace WebApi.Schemas;

public class ConversationNoClients
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid id { get; private set; }
    public string source_id { get; private set; }

    [BsonRepresentation(BsonType.String)]
    public ConversationState state { get; private set; } = ConversationState.Open;
    public List<Log> logs { get; set; } = [];

    public ConversationNoClients(string sourceId, Log log)
    {
        id = Guid.NewGuid();
        source_id = sourceId;
        logs.Add(log);
    }

    public ConversationNoClients(string activeChannel, string sourceId, Log log)
    {
        id = Guid.NewGuid();
        source_id = sourceId;
        logs.Add(log);
    }
}
namespace WebApi.Schemas;

public class Conversation
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid id { get; private set; }
    public string? codigo_cliente { get; private set; }
    public string active_channel { get; private set; }
    public string source_id { get; private set; }

    [BsonRepresentation(BsonType.String)]
    public ConversationState state { get; private set; } = ConversationState.Open;
    public UserDetails? user_details { get; private set; }
    public List<Log> logs { get; set; } = [];

    public Conversation(string activeChannel, string sourceId, string? codigoCliente, UserDetails? userDetails, Log log)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(codigoCliente);
        ArgumentNullException.ThrowIfNull(userDetails);

        id = Guid.NewGuid();
        codigo_cliente = codigoCliente;
        active_channel = activeChannel;
        source_id = sourceId;
        user_details = userDetails;
        logs.Add(log);
    }

    public Conversation(string activeChannel, string sourceId, Log log)
    {
        id = Guid.NewGuid();
        active_channel = activeChannel;
        source_id = sourceId;
        logs.Add(log);
    }
}

﻿using WebApi.Enums;

namespace WebApi.Schemas;

public class IdentifiedCustomer
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid id { get; private set; }
    public string codigo_cliente { get; private set; }

    [BsonRepresentation(BsonType.String)]
    public Channel active_channel { get; private set; }
    public string source_id { get; private set; }

    [BsonRepresentation(BsonType.String)]
    public ManageBy manage_by { get; private set; } = ManageBy.AIAgent;
    public UserDetails? user_details { get; private set; }
    public List<Log> logs { get; set; } = [];

    public IdentifiedCustomer(string sourceId, string codigoCliente, UserDetails? userDetails, Log log, Channel activeChannel = Channel.WhatsApp)
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

    public IdentifiedCustomer(string sourceId, Log log, Channel activeChannel = Channel.WhatsApp)
    {
        id = Guid.NewGuid();
        active_channel = activeChannel;
        source_id = sourceId;
        logs.Add(log);
    }
}

﻿namespace WebApi.Schemas.Logs;

[BsonDiscriminator(RootClass = true)]
[BsonKnownTypes(typeof(AIAgentTextLog), typeof(AgentTextLog), typeof(AssignmentLog), typeof(CommentLog), typeof(TicketStateLog), typeof(WhatsAppTextLog))]
public class Log(Guid id, DateTime? timestamp = null)
{
    [BsonRepresentation(BsonType.String)]
    public Guid log_id { get; set; } = id;
    public DateTime timestamp { get; set; } = timestamp ?? DateTime.Now;
}
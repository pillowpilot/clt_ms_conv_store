﻿namespace ConvCrmContracts.Crm.Events;

public class TicketCommentAdded : IBaseEvent
{
    public string type { get; set; } = default!;
    public string uuid { get; set; } = default!;
    public DateTime timestamp { get; set; }
    public string ticket_id { get; set; } = default!;
    public string comment { get; set; } = default!;
    public AuthorizedBy authorized_by { get; set; } = new();
}
﻿namespace WebApi.Integration.Events.Consumers;

public class TicketStateChangedConsumer(MongoDBService mongo) : IConsumer<TicketStateChanged>
{
    public async Task Consume(ConsumeContext<TicketStateChanged> context)
    {
        var @event = context.Message;

        var sender = new AgentSender(@event.authorized_by.agent_name, @event.authorized_by.agent_id.ToString());
        var ticketStateLog = new TicketStateLog(@event.ticket_id, sender, @event.timestamp, @event.new_state);

        var filterIdentified = Builders<IdentifiedCustomer>.Filter.Eq(conv => conv.id, @event.conversation_id);
        var filterAnonymous = Builders<AnonymousCustomer>.Filter.Eq(conv => conv.id, @event.conversation_id);

        var identifiedConversation = await mongo.IdentifiedCustomers.Find(filterIdentified).FirstOrDefaultAsync();
        var conversationAnonymous = await mongo.AnonymousCustomers.Find(filterAnonymous).FirstOrDefaultAsync();

        if (identifiedConversation is not null)
        {
            var filterUpdate = Builders<IdentifiedCustomer>.Update.Push(conv => conv.logs, ticketStateLog);

            await mongo.IdentifiedCustomers.UpdateOneAsync(filterIdentified, filterUpdate);
        }

        if (conversationAnonymous is not null)
        {
            var filterUpdate = Builders<AnonymousCustomer>.Update.Push(conv => conv.logs, ticketStateLog);

            await mongo.AnonymousCustomers.UpdateOneAsync(filterAnonymous, filterUpdate);
        }
    }
}

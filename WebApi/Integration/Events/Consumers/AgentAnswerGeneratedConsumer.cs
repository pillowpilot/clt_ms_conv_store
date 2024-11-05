namespace WebApi.Integration.Events.Consumers;

public class AgentAnswerGeneratedConsumer(MongoDBService mongo, ISendEndpointProvider endpointProvider) : IConsumer<AgentAnswerGenerated>
{
    public async Task Consume(ConsumeContext<AgentAnswerGenerated> context)
    {
        var @event = context.Message;

        var sender = new AgentSender(@event.agent_details.name, @event.agent_details.id.ToString());

        var agentTextLog = new AgentTextLog(@event.body, sender, @event.timestamp);

        var filterIdentified = Builders<IdentifiedCustomer>.Filter.Eq(conv => conv.id, @event.conversation_id);
        var filterAnonymous = Builders<AnonymousCustomer>.Filter.Eq(conv => conv.id, @event.conversation_id);

        var identifiedConversation = await mongo.IdentifiedCustomers.Find(filterIdentified).FirstOrDefaultAsync();
        var conversationAnonymous = await mongo.AnonymousCustomers.Find(filterAnonymous).FirstOrDefaultAsync();

        if (identifiedConversation is not null)
        {
            var filterUpdate = Builders<IdentifiedCustomer>.Update.Push(conv => conv.logs, agentTextLog);

            await mongo.IdentifiedCustomers.UpdateOneAsync(filterIdentified, filterUpdate);

            //TODO: de donde sacar el sender y el receiver para enviar de vuelta al whatsapp
            var command = new SendWABATextMessage(@event.uuid, @event.timestamp, identifiedConversation.source_id, identifiedConversation.source_id, @event.body);
            await endpointProvider.Send(nameof(SendWABATextMessage), command);
        }

        if (conversationAnonymous is not null)
        {
            var filterUpdate = Builders<AnonymousCustomer>.Update.Push(conv => conv.logs, agentTextLog);

            await mongo.AnonymousCustomers.UpdateOneAsync(filterAnonymous, filterUpdate);

            //TODO: de donde sacar el sender y el receiver para enviar de vuelta al whatsapp
            var command = new SendWABATextMessage(@event.uuid, @event.timestamp, conversationAnonymous.source_id, conversationAnonymous.source_id, @event.body);
            await endpointProvider.Send(nameof(SendWABATextMessage), command);
        }
    }
}

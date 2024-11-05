namespace WebApi.Integration.Events.Consumers;

public class AgentAssignedConsumer(MongoDBService mongo) : IConsumer<AgentAssigned>
{
    public async Task Consume(ConsumeContext<AgentAssigned> context)
    {
        var @event = context.Message;

        var newAgent = new AgentSender(@event.agent_assigned.agent_name, @event.agent_assigned.agent_id.ToString());
        var by = new AgentSender(@event.authorized_by.agent_name, @event.authorized_by.agent_id.ToString());

        var assignmentLog = new AssignmentLog(newAgent, by, @event.timestamp);

        var filterIdentified = Builders<IdentifiedCustomer>.Filter.Eq(conv => conv.id, @event.conversation_id);
        var filterAnonymous = Builders<AnonymousCustomer>.Filter.Eq(conv => conv.id, @event.conversation_id);

        var identifiedConversation = await mongo.IdentifiedCustomers.Find(filterIdentified).FirstOrDefaultAsync();
        var conversationAnonymous = await mongo.AnonymousCustomers.Find(filterAnonymous).FirstOrDefaultAsync();

        if (identifiedConversation is not null)
        {
            var filterUpdate = Builders<IdentifiedCustomer>.Update.Push(conv => conv.logs, assignmentLog);

            await mongo.IdentifiedCustomers.UpdateOneAsync(filterIdentified, filterUpdate);
        }

        if (conversationAnonymous is not null)
        {
            var filterUpdate = Builders<AnonymousCustomer>.Update.Push(conv => conv.logs, assignmentLog);

            await mongo.AnonymousCustomers.UpdateOneAsync(filterAnonymous, filterUpdate);
        }
    }
}

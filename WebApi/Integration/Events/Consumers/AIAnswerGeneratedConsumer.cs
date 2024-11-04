namespace WebApi.Integration.Events.Consumers;

public class AIAnswerGeneratedConsumer(MongoDBService mongo, ISendEndpointProvider endpointProvider, IRequestClient<TicketCreated> requestClient) : IConsumer<AIAnswerGenerated>
{
    public async Task Consume(ConsumeContext<AIAnswerGenerated> context)
    {
        var @event = context.Message;
        var aiMessage = new AIAgentTextLog(@event.body, @event.timestamp);

        var filter = Builders<AnonymousCustomer>.Filter.Eq(conv => conv.id, @event.conversation_id);

        var conversation = await mongo.AnonymousCustomers.Find(filter).FirstOrDefaultAsync();

        if (conversation is null) return;

        var filterUpdate = Builders<AnonymousCustomer>.Update.Push(conv => conv.logs, aiMessage);

        await mongo.AnonymousCustomers.UpdateOneAsync(filter, filterUpdate);


        if (@event.open_ticket)
        {
            var command = new OpenTicket(@event.uuid, conversation.source_id, Channel.WhatsApp.ToString(), null);
            //await endpointProvider.Send(nameof(OpenTicket), command);
            var ticketCreated = await requestClient.GetResponse<TicketCreated>(command);

            //Log de pending
            var ticketStateLog = new TicketStateLog(ticketCreated.Message.ticket_number, new AgentSender("BOT"));

            var filterManagedBy = Builders<AnonymousCustomer>.Update
                .Set(conv => conv.manage_by, ManageBy.Agent)
                .Push(x => x.logs, ticketStateLog);

            await mongo.AnonymousCustomers.UpdateOneAsync(filter, filterManagedBy);
        }
        else
        {
            //TODO: de donde sacar el sender y el receiver para enviar de vuelta al whatsapp
            var command = new SendWABATextMessage(@event.uuid, @event.timestamp, conversation.source_id, conversation.source_id, @event.body);
            await endpointProvider.Send(nameof(SendWABATextMessage), command);
        }
    }
}

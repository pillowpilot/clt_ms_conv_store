using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace WebApi.Integration.Events.Consumers;

public class AIAnswerGeneratedWithUserInfoConsumer(MongoDBService mongo, ISendEndpointProvider endpointProvider, IBus bus, ILogger<AIAnswerGeneratedWithUserInfoConsumer> logger) : IConsumer<AIAnswerGeneratedWithUserInfo>
{
    public async Task Consume(ConsumeContext<AIAnswerGeneratedWithUserInfo> context)
    {
        var @event = context.Message;
        var aiMessage = new AIAgentTextLog(@event.body, @event.timestamp);

        var filter = Builders<IdentifiedCustomer>.Filter.Eq(conv => conv.id, @event.conversation_id);

        var conversation = await mongo.IdentifiedCustomers.Find(filter).FirstOrDefaultAsync();

        if (conversation is null) return;

        var filterUpdate = Builders<IdentifiedCustomer>.Update.Push(conv => conv.logs, aiMessage);

        await mongo.IdentifiedCustomers.UpdateOneAsync(filter, filterUpdate);


        if (@event.open_ticket)
        {
            var command = new OpenTicket(@event.uuid, conversation.source_id, Channel.WhatsApp.ToString(), conversation.user_details);
             var ticketCreated = await bus.GetResponse<OpenTicket, TicketCreated>(nameof(OpenTicket), command);

            //Log de pending
            var ticketStateLog = new TicketStateLog(ticketCreated.ticket_id, new AgentSender("BOT"), ticketCreated.timestamp);

            var filterManagedBy = Builders<IdentifiedCustomer>.Update
                .Set(conv => conv.manage_by, ManageBy.Agent)
                .Push(x => x.logs, ticketStateLog);

            await mongo.IdentifiedCustomers.UpdateOneAsync(filter, filterManagedBy);
        }
        else
        {
            //TODO: de donde sacar el sender y el receiver para enviar de vuelta al whatsapp
            var command = new SendWABATextMessage(@event.uuid, @event.timestamp, conversation.source_id, conversation.source_id, @event.body);
            await endpointProvider.Send(nameof(SendWABATextMessage), command);
        }
    }
}
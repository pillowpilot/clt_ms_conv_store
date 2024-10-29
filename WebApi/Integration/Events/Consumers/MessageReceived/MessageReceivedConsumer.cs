using System.Text.Json;
using System.Text.Json.Serialization;
using WebApi.Integration.Commands.OpenTicket;

namespace WebApi.Integration.Events.Consumers.MessageReceived;

public class MessageReceivedEventConsumer(MongoDBService mongo, IPublishEndpoint publishEndpoint) : IConsumer<MessageReceivedEvent>
{
    public async Task Consume(ConsumeContext<MessageReceivedEvent> context)
    {
        var @event = context.Message;
        var fallbackText = @event.body;
        var userDetails = @event.user_details;

        //Crear sender
        var sender = new WhatsAppSender(userDetails!.name, @event.sender);

        //Crear log de mensaje via whatsapp
        var whatsAppMessage = new WhatsAppTextLog(@event.body, sender, @event.timestamp);

        //Filtro para buscar conversacion por el source_id y en estado abierto
        var filter = Builders<Conversation>.Filter.And(
            Builders<Conversation>.Filter.Eq(conv => conv.source_id, @event.source_id),
            Builders<Conversation>.Filter.Eq(conv => conv.state, ConversationState.Open)
        );

        //Recuperar conversacion, si existe, agregar nuevo log, sino, crear conversacion nueva
        var conversation = await mongo.Conversations.Find(filter).FirstOrDefaultAsync();

        if (conversation is null)
        {
            await HandleNewConversation(@event, userDetails, whatsAppMessage);
        }
        else
        {
            await HandleExistingConversation(conversation, filter, whatsAppMessage);
        }

        //ASK: una vez guardado un message receive, ya sea para crear una conversacion nueva, o agregar nuevo log a uno existente, el crm
        //como sabe que hay un mensaje para mostrar, el conv-store tiene que disparar un evento aca mismo?
    }

    //Crear nueva conversacion y enviar comando para crear un ticket con dicha conversacion
    private async Task HandleNewConversation(MessageReceivedEvent @event, UserDetails? userDetails, WhatsAppTextLog whatsAppMessage)
    {
        var conversation = new Conversation(@event.channel, @event.source_id, userDetails?.codigo_cliente, userDetails, whatsAppMessage);
        await mongo.Conversations.InsertOneAsync(conversation);

        var command = OpenTicketCommand.Create(conversation.id, conversation.user_details);
        await publishEndpoint.Publish(command);
    }

    //Agregar nuevo log de mensaje a la conversacion
    private async Task HandleExistingConversation(Conversation conversation, FilterDefinition<Conversation> filter, WhatsAppTextLog whatsAppMessage)
    {
        var filterUpdate = Builders<Conversation>.Update.Set(conv => conv.logs, [.. conversation.logs, whatsAppMessage]);

        await mongo.Conversations.UpdateOneAsync(filter, filterUpdate);
    }
}

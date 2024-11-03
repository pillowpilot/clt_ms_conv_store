using System.Text.Json;
using WebApi.Extensions;

namespace WebApi.Integration.Events.Consumers;

public class MessageReceivedEventConsumer(MongoDBService mongo, ISendEndpointProvider endpointProvider,
 ILogger<MessageReceivedEventConsumer> logger, MessagePublisherService messagePublisherService) : IConsumer<MessageReceivedEvent>
{
    public async Task Consume(ConsumeContext<MessageReceivedEvent> context)
    {
        logger.LogInformation("Escuchando mensajes en RabbitMQ...");
        var @event = context.Message;
        var userDetailsJson = JsonSerializer.Serialize(@event.user_details);
        logger.LogInformation("Mensaje recibido: {Message}", @event.body);
        var fallbackText = @event.body;
        var userDetails = @event.user_details;

        //Crear sender
        var sender = new WhatsAppSender(userDetails!.name, @event.sender);

        //Crear log de mensaje via whatsapp
        var whatsAppMessage = new WhatsAppTextLog(@event.body, sender, @event.timestamp);

        //Filtro para buscar conversacion por el source_id y en estado abierto
        var filter = Builders<ConversationClients>.Filter.And(
            Builders<ConversationClients>.Filter.Eq(conv => conv.source_id, @event.source_id),
            Builders<ConversationClients>.Filter.Eq(conv => conv.state, ConversationState.Open)
        );

        //Recuperar conversacion, si existe, agregar nuevo log, sino, crear conversacion nueva
        var conversation = await mongo.ConversationsClients.Find(filter).FirstOrDefaultAsync();

        if (conversation is null)
        {
            logger.LogInformation("No se encontró una conversación. Creando una nueva...");
            await HandleNewConversation(@event, userDetails, whatsAppMessage);
            await messagePublisherService.SendMessageAsync(whatsAppMessage, "AIMessage");
            logger.LogInformation("Conversación enviada al agente IA.");
        }
        else
        {
            logger.LogInformation("Conversación existente encontrada. Agregando mensaje al log...");
            await HandleExistingConversation(conversation, filter, whatsAppMessage);
            await messagePublisherService.SendMessageAsync(whatsAppMessage, "AIMessageHist");
            logger.LogInformation("Conversación enviada al agente IA.");
        }
    }

    //Crear nueva conversacion y enviar comando para crear un ticket con dicha conversacion
    private async Task HandleNewConversation(MessageReceivedEvent @event, UserDetails? userDetails, WhatsAppTextLog whatsAppMessage)
    {
        var conversation = new ConversationClients(@event.channel, @event.source_id, userDetails?.codigo_cliente, userDetails, whatsAppMessage);
        try
        {
            await mongo.ConversationsClients.InsertOneAsync(conversation);
        }
        catch (Exception ex)
        {
            logger.LogError("Error al insertar en MongoDB: {Error}", ex.Message);
        }

        var command = OpenTicketCommand.Create(conversation.id, conversation.source_id, conversation.active_channel, conversation.user_details);
        await endpointProvider.Send(nameof(OpenTicketCommand), command);
    }

    //Agregar nuevo log de mensaje a la conversacion
    private async Task HandleExistingConversation(ConversationClients conversation, FilterDefinition<ConversationClients> filter, WhatsAppTextLog whatsAppMessage)
    {
        var filterUpdate = Builders<ConversationClients>.Update.Set(conv => conv.logs, [.. conversation.logs, whatsAppMessage]);
        try
        {
            await mongo.ConversationsClients.UpdateOneAsync(filter, filterUpdate);
        }
        catch (Exception ex)
        {
            logger.LogError("Error al insertar en MongoDB: {Error}", ex.Message);
        }

    }
}

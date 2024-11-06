using WebApi.Schemas.Tickets;

namespace WebApi.Integration.Events.Consumers;

public class WABATextMsgWithUserInfoConsumer(MongoDBService mongo, ISendEndpointProvider endpointProvider,
    ILogger<WABATextMsgWithUserInfoConsumer> logger)
    : IConsumer<TicketEvent>
{
    public async Task Consume(ConsumeContext<TicketEvent> context)
    {
        logger.LogInformation("Escuchando mensajes en RabbitMQ...");
        var ticketEvent = context.Message;

        logger.LogInformation("Mensaje recibido: {Message}", context);

        //Filtro para buscar conversacion por el source_id y en estado abierto
        var filter = Builders<TicketIdentifiedCustomer>.Filter.Or(
            Builders<TicketIdentifiedCustomer>.Filter.Eq(conv => conv.CustomerNumber, ticketEvent.UserData.CustomerPhone),
            Builders<TicketIdentifiedCustomer>.Filter.Eq(conv => conv.CustomerId, ticketEvent.UserData.CustormerId ?? "cod_not_found")
        );

        //Recuperar conversacion, si existe, agregar nuevo log, sino, crear conversacion nueva
        var conversation = await mongo.TicketIdentifiedCustomer.Find(filter).FirstOrDefaultAsync();

        if (conversation is null)
        {
            logger.LogInformation("No se encontró una conversación. Creando una nueva...");

            var newConversation = await HandleNewConversation(ticketEvent);

            if (newConversation is null) return;

            //Crear comando y enviar para generacion de respuesta de la IA
            //await messagePublisherService.SendMessageAsync(whatsAppMessage, "AIMessage");
            /* var command = new GenerateAIAnswerWithUserInfo(newConversation.id, whatsAppMessage.timestamp, userDetails, BuildMessage(newConversation.id, whatsAppMessage));
             await endpointProvider.Send(nameof(GenerateAIAnswerWithUserInfo), command);
             */
            logger.LogInformation("Comando enviada al agente IA.");
        }
        else
        {
            logger.LogInformation("Conversación existente encontrada. Agregando mensaje al log...");

            await HandleExistingConversation(conversation, filter, ticketEvent);

            /* if (conversation.manage_by is ManageBy.AIAgent)
             {
                 //await messagePublisherService.SendMessageAsync(whatsAppMessage, "AIMessageHist");

                 var command = new GenerateAIAnswerWithUserInfo(conversation.id, whatsAppMessage.timestamp, userDetails, BuildMessage(conversation.id, whatsAppMessage));
                 await endpointProvider.Send(nameof(GenerateAIAnswerWithUserInfo), command);

                 logger.LogInformation("Comando enviada al agente IA.");
             }
             else //Enviar al agente
             {
                 var command = new GenerateAgentAnswerWithUserInfo(conversation.id, whatsAppMessage.timestamp, userDetails, BuildMessage(conversation.id, whatsAppMessage));
                 await endpointProvider.Send(nameof(GenerateAgentAnswerWithUserInfo), command);

                 logger.LogInformation("Comando enviada al agente.");
             }*/
        }
    }

    //Crear nueva conversacion y enviar comando para crear un ticket con dicha conversacion
    private async Task<TicketIdentifiedCustomer?> HandleNewConversation(TicketEvent ticketEvent)
    {
        try
        {
            var conversation = new TicketIdentifiedCustomer(ticketEvent);

            await mongo.TicketIdentifiedCustomer.InsertOneAsync(conversation);

            return conversation;
        }
        catch (Exception ex)
        {
            logger.LogError("Error al insertar en MongoDB: {Error}", ex.Message);
            return null;
        }
    }

    //Agregar nuevo log de mensaje a la conversacion
    private async Task HandleExistingConversation(TicketIdentifiedCustomer conversation, FilterDefinition<TicketIdentifiedCustomer> filter, TicketEvent ticketEvent)
    {
        var update = Builders<TicketIdentifiedCustomer>.Update.Push("messages", ticketEvent.Message);
        try
        {
            await mongo.TicketIdentifiedCustomer.UpdateOneAsync(filter, update);
        }
        catch (Exception ex)
        {
            logger.LogError("Error al insertar en MongoDB: {Error}", ex.Message);
        }

    }

    private Conversation BuildMessage(Guid id, WhatsAppTextLog whatsAppTextLog)
    {
        return new Conversation(id, whatsAppTextLog.fallback_text);
    }
}

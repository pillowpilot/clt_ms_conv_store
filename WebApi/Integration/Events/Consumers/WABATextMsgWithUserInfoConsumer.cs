namespace WebApi.Integration.Events.Consumers;

public class WABATextMsgWithUserInfoConsumer(MongoDBService mongo, ISendEndpointProvider endpointProvider,
    ILogger<WABATextMsgWithUserInfoConsumer> logger)
    : IConsumer<WABATextMsgWithUserInfo>
{
    public async Task Consume(ConsumeContext<WABATextMsgWithUserInfo> context)
    {
        logger.LogInformation("Escuchando mensajes en RabbitMQ...");
        var @event = context.Message;

        logger.LogInformation("Mensaje recibido: {Message}", @event.body);
        var fallbackText = @event.body;
        var userDetails = @event.user_details;

        //Crear sender
        var sender = new WhatsAppSender(userDetails!.name, @event.sender);

        //Crear log de mensaje via whatsapp
        var whatsAppMessage = new WhatsAppTextLog(@event.body, sender, @event.timestamp);

        //Filtro para buscar conversacion por el source_id y en estado abierto
        var filter = Builders<IdentifiedCustomer>.Filter.Or(
            Builders<IdentifiedCustomer>.Filter.Eq(conv => conv.source_id, @event.sender),
            Builders<IdentifiedCustomer>.Filter.Eq(conv => conv.user_details!.codigo_cliente, @event.user_details?.codigo_cliente ?? "cod_not_found")
        );

        //Recuperar conversacion, si existe, agregar nuevo log, sino, crear conversacion nueva
        var conversation = await mongo.IdentifiedCustomers.Find(filter).FirstOrDefaultAsync();

        if (conversation is null)
        {
            logger.LogInformation("No se encontró una conversación. Creando una nueva...");

            var newConversation = await HandleNewConversation(@event, userDetails, whatsAppMessage);

            if (newConversation is null) return;

            //Crear comando y enviar para generacion de respuesta de la IA
            //await messagePublisherService.SendMessageAsync(whatsAppMessage, "AIMessage");
            var command = new GenerateAIAnswerWithUserInfo(newConversation.id, whatsAppMessage.timestamp, userDetails, BuildMessage(newConversation.id, whatsAppMessage));
            await endpointProvider.Send(nameof(GenerateAIAnswerWithUserInfo), command);

            logger.LogInformation("Comando enviada al agente IA.");
        }
        else
        {
            logger.LogInformation("Conversación existente encontrada. Agregando mensaje al log...");

            await HandleExistingConversation(filter, whatsAppMessage);

            if (conversation.manage_by is ManageBy.AIAgent)
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
            }
        }
    }

    //Crear nueva conversacion y enviar comando para crear un ticket con dicha conversacion
    private async Task<IdentifiedCustomer?> HandleNewConversation(WABATextMsgWithUserInfo @event, UserDetails? userDetails, WhatsAppTextLog whatsAppMessage)
    {
        try
        {
            var conversation = new IdentifiedCustomer(@event.sender, userDetails!.codigo_cliente, userDetails, whatsAppMessage);

            await mongo.IdentifiedCustomers.InsertOneAsync(conversation);

            return conversation;
        }
        catch (Exception ex)
        {
            logger.LogError("Error al insertar en MongoDB: {Error}", ex.Message);
            return null;
        }
    }

    //Agregar nuevo log de mensaje a la conversacion
    private async Task HandleExistingConversation(FilterDefinition<IdentifiedCustomer> filter, WhatsAppTextLog whatsAppMessage)
    {
        var filterUpdate = Builders<IdentifiedCustomer>.Update.Push(conv => conv.logs, whatsAppMessage);
        try
        {
            await mongo.IdentifiedCustomers.UpdateOneAsync(filter, filterUpdate);
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

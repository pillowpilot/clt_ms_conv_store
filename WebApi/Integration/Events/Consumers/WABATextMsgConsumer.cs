namespace WebApi.Integration.Events.Consumers;

public class WABATextMsgConsumer(MongoDBService mongo,
 ILogger<WABATextMsgConsumer> logger, MessagePublisherService messagePublisher) : IConsumer<WABATextMsg>
{
    public async Task Consume(ConsumeContext<WABATextMsg> context)
    {
        logger.LogInformation("Escuchando mensajes en RabbitMQ...");
        var @event = context.Message;

        logger.LogInformation("Mensaje recibido: {Message}", @event.body);
        var fallbackText = @event.body;

        @event.source_id = @event.sender;

        //Crear sender
        var sender = new WhatsAppSender(null, @event.sender);

        //Crear log de mensaje via whatsapp
        var whatsAppMessage = new WhatsAppTextLog(@event.body, sender, @event.timestamp);

        //Filtro para buscar conversacion por el source_id y en estado abierto
        var filter = Builders<AnonymousCustomer>.Filter.And(
            Builders<AnonymousCustomer>.Filter.Eq(conv => conv.source_id, @event.source_id),
            Builders<AnonymousCustomer>.Filter.Eq(conv => conv.manage_by, ManageBy.AIAgent)
        );

        //Recuperar conversacion, si existe, agregar nuevo log, sino, crear conversacion nueva
        var conversation = await mongo.AnonymousCustomers.Find(filter).FirstOrDefaultAsync();

        if (conversation is null)
        {
            logger.LogInformation("No se encontró una conversación. Creando una nueva...");
            await HandleNewConversation(@event, whatsAppMessage);
            await messagePublisher.SendMessageAsync(whatsAppMessage, "solucion");
            logger.LogInformation("Conversación enviada al agente IA.");
        }
        else
        {
            logger.LogInformation("Conversación existente encontrada. Agregando mensaje al log.");
            await HandleExistingConversation(conversation, filter, whatsAppMessage);
            await messagePublisher.SendMessageAsync(conversation, "prueba");
            logger.LogInformation("Conversación enviada al agente IA.");
        }
    }

    //Crear nueva conversacion y enviar comando para crear un ticket con dicha conversacion
    private async Task HandleNewConversation(WABATextMsg @event, WhatsAppTextLog whatsAppMessage)
    {
        var conversation = new AnonymousCustomer(@event.source_id, whatsAppMessage);
        try
        {
            await mongo.AnonymousCustomers.InsertOneAsync(conversation);
            logger.LogInformation("Inserto correctamente...");
        }
        catch (Exception ex)
        {
            logger.LogError("Error al insertar en MongoDB: {Error}", ex.Message);
        }
    }

    //Agregar nuevo log de mensaje a la conversacion
    private async Task HandleExistingConversation(AnonymousCustomer conversation, FilterDefinition<AnonymousCustomer> filter, WhatsAppTextLog whatsAppMessage)
    {
        var filterUpdate = Builders<AnonymousCustomer>.Update.Set(conv => conv.logs, [.. conversation.logs, whatsAppMessage]);
        try
        {
            await mongo.AnonymousCustomers.UpdateOneAsync(filter, filterUpdate);
        }
        catch (Exception ex)
        {
            logger.LogError("Error al actualizar en MongoDB: {Error}", ex.Message);
        }

    }
}

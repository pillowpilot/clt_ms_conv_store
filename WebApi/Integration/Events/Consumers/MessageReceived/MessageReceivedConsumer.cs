namespace WebApi.Integration.Events.Consumers.MessageReceived;

public class MessageReceivedEventConsumer(MongoDBService mongo) : IConsumer<MessageReceivedEvent>
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

        //Filtro para buscar conversacion por el source_id
        var filter = Builders<Conversation>.Filter.Eq(conv => conv.source_id, @event.source_id);

        //Recuperar conversacion, si existe, agregar nuevo log, sino, crear conversacion nueva
        var conversation = await mongo.Conversations.Find(filter).FirstOrDefaultAsync();

        if (conversation is null)
        {
            var document = new Conversation(@event.channel, @event.source_id, userDetails.codigo_cliente, userDetails, whatsAppMessage);
            await mongo.Conversations.InsertOneAsync(document);
        }
        else
        {
            //Se agrega nuevo log
            var filterUpdate = Builders<Conversation>.Update.Set(conv => conv.logs, [.. conversation.logs, whatsAppMessage]);

            await mongo.Conversations.UpdateOneAsync(filter, filterUpdate);
        }

        //ASK: una vez guardado un message receive, ya sea para crear una conversacion nueva, o agregar nuevo log a uno existente, el crm
        //como sabe que hay un mensaje para mostrar, el conv-store tiene que disparar un evento aca mismo?
    }
}

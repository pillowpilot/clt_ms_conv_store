using ConvCrmContracts.Conv.Querys;
using MassTransit;
using System.Collections.Generic;
using System.Text.Json;

namespace WebApi.Integration.Events.Consumers;

public class GetConversationConsumer(MongoDBService mongo, ILogger<GetConversationConsumer> logger) : IConsumer<GetConversation>
{
    public async Task Consume(ConsumeContext<GetConversation> context)
    {
        var @event = context.Message;

        logger.LogInformation("El id a consultar es: {Message}", @event.conversation_id);
        logger.LogInformation("Fecha Inicio: {inicio}", @event.start_timestamp);
        logger.LogInformation("Fecha Fin: {fin}", @event.end_timestamp);

        var filtroSourceId = Builders<IdentifiedCustomer>.Filter.Eq(conv => conv.id, @event.conversation_id);

        //Comente esta parte del rango de fecha porque no esta filtrando
        //var filtroFechaLogs = Builders<IdentifiedCustomer>.Filter.ElemMatch(
        //    conv => conv.logs,
        //    log => log.timestamp >= @event.start_timestamp && log.timestamp <= @event.end_timestamp);

        var filtro = Builders<IdentifiedCustomer>.Filter.And(filtroSourceId);

        var resultado = await mongo.IdentifiedCustomers.Find(filtro).FirstOrDefaultAsync();

        //Extraer el indice del log cuando se cerro el ticket
        int desiredIndex = GetDesiredIndex(resultado.logs, @event.ticket_id);

        //Extrar los mensajes a partir del indice deseado hasta el cierre de un ticket anterior
        List<Log> extractedLogs = GetMessages(resultado.logs, desiredIndex, @event.ticket_id);

        //Extraer los mensajes del User (de whatsapp), IA y del Agente
        var ai = extractedLogs.OfType<AIAgentTextLog>().Select(x => new ConversationMessages(x.log_id, x.sender.agent_id, x.sender.display_name, x.fallback_text, x.timestamp));
        var agent = extractedLogs.OfType<AgentTextLog>().Select(x => new ConversationMessages(x.log_id, x.sender.agent_id, x.sender.display_name, x.fallback_text, x.timestamp));
        var user = extractedLogs.OfType<WhatsAppTextLog>().Select(x => new ConversationMessages(x.log_id, null, x.sender.display_name, x.fallback_text, x.timestamp, false));

        //Juntar y ordenar mensajes
        var messages = ai.Concat(agent).Concat(user)
            .OrderBy(x => x.time)
            .ToList();

        //Formalizar response
        var conversationDocument = new ConversationDocument(resultado.id, resultado.codigo_cliente, resultado.active_channel.ToString(), resultado.source_id, resultado.user_details, messages);

        await context.RespondAsync(conversationDocument);

        logger.LogInformation("Resultado de la consulta: {Resultado}", JsonSerializer.Serialize(conversationDocument, new JsonSerializerOptions { WriteIndented = true }));
    }

    private int GetDesiredIndex(List<Log> logs, int ticketId)
    {
        var desiredIndex = 0;
        for (int i = 0; i < logs.Count; i++)
        {
            var log = logs[i];

            if (log is TicketStateLog item && item.new_state == "closed" && item.ticket_id == ticketId)
            {
                desiredIndex = i;
                break;
            }
        }

        return desiredIndex;
    }    
    
    private List<Log> GetMessages(List<Log> logs, int desiredIndex, int ticketId)
    {
        var extractedLogs = new List<Log>();
        for (int i = desiredIndex; i >= 0; i--)
        {
            var log = logs[i];

            if (log is TicketStateLog item && item.new_state == "closed" && item.ticket_id != ticketId)
            {
                break;
            }

            extractedLogs.Add(log);
        }

        return extractedLogs;
    }
}

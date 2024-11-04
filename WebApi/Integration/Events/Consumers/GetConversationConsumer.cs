using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ConvCrmContracts.Conv.Querys;

namespace WebApi.Integration.Events.Consumers
{
    public class GetConversationConsumer(MongoDBService mongo, ISendEndpointProvider endpointProvider, ILogger<GetConversationConsumer> logger) : IConsumer<GetConversationQuery>
    {
        public async Task Consume(ConsumeContext<GetConversationQuery> context)
        {
            var @event = context.Message;
            logger.LogInformation("El id a consultar es: {Message}", @event.source_id);
            logger.LogInformation("Fecha Inicio: {inicio}", @event.start_timestamp);
            logger.LogInformation("Fecha Fin: {fin}", @event.end_timestamp);
            var filtroSourceId = Builders<AnonymousCustomer>.Filter.Eq(conv => conv.source_id, @event.source_id);

            var filtroFechaLogs = Builders<AnonymousCustomer>.Filter.ElemMatch(
                conv => conv.logs,
                log => log.timestamp >= @event.start_timestamp && log.timestamp <= @event.end_timestamp);
            var filtro = Builders<AnonymousCustomer>.Filter.And(filtroSourceId);

            var resultado = await mongo.AnonymousCustomers.Find(filtro).ToListAsync();
            logger.LogInformation("Resultado de la consulta: {Resultado}", JsonSerializer.Serialize(resultado, new JsonSerializerOptions { WriteIndented = true }));
        }
    }
}

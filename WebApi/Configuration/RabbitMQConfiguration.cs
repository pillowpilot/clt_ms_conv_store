using System.Net.Mime;
using ConvCrmContracts.Conv.Querys;
using WebApi.Integration.Events.Consumers;

namespace WebApi.Configuration;

public static class RabbitMQConfiguration
{
    public static IServiceCollection AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddMassTransit(busConfig =>
        {
            busConfig.UsingRabbitMq((context, rabbitConfig) =>
            {
                var messageBroker = configuration.GetSection(nameof(MessageBroker)).Get<MessageBroker>()!;

                rabbitConfig.Host(new Uri(messageBroker.Host), host =>
                {
                    host.Username(messageBroker.Username);
                    host.Password(messageBroker.Password);
                });

                //Ignorar todas las serializacion y utilizar solo RawJson
                rabbitConfig.ClearSerialization();
                rabbitConfig.UseRawJsonSerializer();

                rabbitConfig.AddQueues(context);
            });

            busConfig.AddConsumers();
        });
    }

    //Configurar los queues de sus respectivos consumers
    private static void AddQueues(this IRabbitMqBusFactoryConfigurator rabbitConfig, IBusRegistrationContext context)
    {
        rabbitConfig.ReceiveEndpoint(nameof(MessageReceivedEvent), ConfigureEndpoint(context, typeof(MessageReceivedEventConsumer)));
        rabbitConfig.ReceiveEndpoint(nameof(MessageReceivedNoClientEvent), ConfigureEndpoint(context, typeof(MessageReceivedConsumerNoClient)));
        rabbitConfig.ReceiveEndpoint(nameof(GetConversationQuery), ConfigureEndpoint(context, typeof(GetConversationConsumer)));
    }

    //Registrar los consumers a utilizar
    private static void AddConsumers(this IBusRegistrationConfigurator busConfig)
    {
        busConfig.AddConsumer<MessageReceivedEventConsumer>();
        busConfig.AddConsumer<MessageReceivedConsumerNoClient>();
        busConfig.AddConsumer<GetConversationConsumer>();
    }

    //Metodo para aplicar una configuracion generica a todos los consumers
    private static Action<IRabbitMqReceiveEndpointConfigurator> ConfigureEndpoint(IBusRegistrationContext context, Type consumer)
    {
        return endpointConfig =>
        {
            endpointConfig.ConfigureConsumeTopology = false;
            endpointConfig.ClearSerialization();
            endpointConfig.UseRawJsonSerializer();
            endpointConfig.DefaultContentType = new ContentType("application/json");
            endpointConfig.ConfigureConsumer(context, consumer);
        };
    }
}

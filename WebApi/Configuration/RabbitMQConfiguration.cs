using ConvCrmContracts.Conv.Querys;
using System.Net.Mime;
using WebApi.Integration.Events.Consumers;

namespace WebApi.Configuration;

public static class RabbitMQConfiguration
{
    public static IServiceCollection AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(busConfig =>
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

        services.AddScoped<MessagePublisherService>();
        return services;
    }

    //Configurar los queues de sus respectivos consumers
    private static void AddQueues(this IRabbitMqBusFactoryConfigurator rabbitConfig, IBusRegistrationContext context)
    {
        rabbitConfig.ReceiveEndpoint(nameof(AgentAnswerGenerated), ConfigureEndpoint(context, typeof(AgentAnswerGeneratedConsumer)));
        rabbitConfig.ReceiveEndpoint(nameof(AgentAssigned), ConfigureEndpoint(context, typeof(AgentAssignedConsumer)));
        rabbitConfig.ReceiveEndpoint(nameof(AIAnswerGenerated), ConfigureEndpoint(context, typeof(AIAnswerGeneratedConsumer)));
        rabbitConfig.ReceiveEndpoint(nameof(AIAnswerGeneratedWithUserInfo), ConfigureEndpoint(context, typeof(AIAnswerGeneratedWithUserInfoConsumer)));
        rabbitConfig.ReceiveEndpoint(nameof(TicketClosed), ConfigureEndpoint(context, typeof(TicketClosedConsumer)));
        rabbitConfig.ReceiveEndpoint(nameof(TicketCommentAdded), ConfigureEndpoint(context, typeof(TicketCommentAddedConsumer)));
        rabbitConfig.ReceiveEndpoint(nameof(TicketOpened), ConfigureEndpoint(context, typeof(TicketOpenedConsumer)));
        rabbitConfig.ReceiveEndpoint(nameof(TicketStateChanged), ConfigureEndpoint(context, typeof(TicketStateChangedConsumer)));
        rabbitConfig.ReceiveEndpoint(nameof(GetConversation), ConfigureEndpoint(context, typeof(GetConversationConsumer)));
        rabbitConfig.ReceiveEndpoint(nameof(WABATextMsg), ConfigureEndpoint(context, typeof(WABATextMsgConsumer)));
        rabbitConfig.ReceiveEndpoint(nameof(WABATextMsgWithUserInfo), ConfigureEndpoint(context, typeof(WABATextMsgWithUserInfoConsumer)));

    }

    //Registrar los consumers a utilizar
    private static void AddConsumers(this IBusRegistrationConfigurator busConfig)
    {
        busConfig.AddConsumer<AgentAnswerGeneratedConsumer>();
        busConfig.AddConsumer<AgentAssignedConsumer>();
        busConfig.AddConsumer<AIAnswerGeneratedConsumer>();
        busConfig.AddConsumer<AIAnswerGeneratedWithUserInfoConsumer>();
        busConfig.AddConsumer<TicketClosedConsumer>();
        busConfig.AddConsumer<TicketCommentAddedConsumer>();
        busConfig.AddConsumer<TicketOpenedConsumer>();
        busConfig.AddConsumer<TicketStateChangedConsumer>();
        busConfig.AddConsumer<GetConversationConsumer>();
        busConfig.AddConsumer<WABATextMsgConsumer>();
        busConfig.AddConsumer<WABATextMsgWithUserInfoConsumer>();
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

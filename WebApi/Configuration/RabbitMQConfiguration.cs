namespace WebApi.Configuration;

public static class RabbitMQConfiguration
{
    public static IServiceCollection AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddMassTransit(busConfig =>
        {
            busConfig.SetKebabCaseEndpointNameFormatter();

            busConfig.UsingRabbitMq((context, rabbitConfig) =>
            {
                var messageBroker = configuration.GetSection(nameof(MessageBroker)).Get<MessageBroker>()!;

                rabbitConfig.Host(new Uri(messageBroker.Host), host =>
                {
                    host.Username(messageBroker.Username);
                    host.Password(messageBroker.Password);
                });

                rabbitConfig.ConfigureEndpoints(context);
            });

            busConfig.AddConsumers(Assembly.GetExecutingAssembly());
        });
    }
}

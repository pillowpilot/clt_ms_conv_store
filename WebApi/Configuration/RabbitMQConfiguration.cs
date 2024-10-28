namespace WebApi.Configuration;

public static class RabbitMQConfiguration
{
    public static IServiceCollection AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddMassTransit(busConfig =>
        {
            busConfig.UsingRabbitMq((context, rabbitConfig) =>
            {
                var messageBroker = context.GetRequiredService<IOptions<MessageBroker>>().Value;
                rabbitConfig.Host(new Uri(messageBroker.Host), host =>
                {
                    host.Username(messageBroker.Username);
                    host.Password(messageBroker.Password);
                });
                rabbitConfig.ConfigureEndpoints(context);
            });

            busConfig.SetKebabCaseEndpointNameFormatter();
            busConfig.AddConsumers(Assembly.GetExecutingAssembly());
        });
    }
}

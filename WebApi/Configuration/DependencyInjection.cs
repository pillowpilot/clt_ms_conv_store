namespace WebApi.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration)
    {
        //return services.AddOptions(configuration);
        return services;
    }

    //private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
    //{
    //    return services.Configure<Database>(configuration.GetSection(nameof(Database)))
    //                   .Configure<MessageBroker>(configuration.GetSection(nameof(MessageBroker)));
    //}
}

using Hkust.Infras.IdGenerater.Yitter;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddHkustInfraYitterIdGenerater(this IServiceCollection services, IConfigurationSection redisSection)
    {
        if (services.HasRegistered(nameof(AddHkustInfraYitterIdGenerater)))
            return services;

        return services
            .AddHkustInfraRedis(redisSection)
            .AddSingleton<WorkerNode>()
            .AddHostedService<WorkerNodeHostedService>();
    }
}
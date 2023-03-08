using Hkust.Infras.EventBus;
using Hkust.Infras.EventBus.Cap;
using Hkust.Infras.EventBus.Cap.Filters;
using Hkust.Infras.EventBus.Configurations;
using Hkust.Infras.EventBus.RabbitMq;
using DotNetCore.CAP;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{

    public static IServiceCollection AddHkustInfraCap<TSubscriber>(this IServiceCollection services, Action<CapOptions> setupAction)
        where TSubscriber : class, ICapSubscribe
    {
        if (services.HasRegistered(nameof(AddHkustInfraCap)))
            return services;
        services
            .AddSingleton<IEventPublisher, CapPublisher>()
            .AddScoped<TSubscriber>()
            .AddCap(setupAction)
            .AddSubscribeFilter<DefaultCapFilter>()
            ;
        return services;
    }


    public static IServiceCollection AddHkustInfraRabbitMq(this IServiceCollection services, IConfigurationSection rabitmqSection)
    {
        if (services.HasRegistered(nameof(AddHkustInfraRabbitMq)))
            return services;

        return services
             .Configure<RabbitMqOptions>(rabitmqSection)
             .AddSingleton<IRabbitMqConnection>(provider =>
             {
                 var options = provider.GetRequiredService<IOptions<RabbitMqOptions>>();
                 var logger = provider.GetRequiredService<ILogger<RabbitMqConnection>>();
                 var serviceInfo = services.GetServiceInfo();
                 var clientProvidedName = serviceInfo?.Id ?? "unkonow";
                 return RabbitMqConnection.GetInstance(options, clientProvidedName, logger);
             })
             .AddSingleton<RabbitMqProducer>()
             ;
    }
}
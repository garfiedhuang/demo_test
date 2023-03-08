using Hkust.Infras.Redis.Caching;
using Hkust.Infras.Redis.Caching.Configurations;
using Hkust.Infras.Redis.Caching.Core.Interceptor;
using Hkust.Infras.Redis.Caching.Interceptor.Castle;
using Hkust.Infras.Redis.Caching.Provider;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddHkustInfraRedisCaching(this IServiceCollection services, IConfigurationSection redisSection, IConfigurationSection cachingSection)
    {
        if (services.HasRegistered(nameof(AddHkustInfraRedisCaching)))
            return services;

        return services
            .AddHkustInfraRedis(redisSection)
            .Configure<CacheOptions>(cachingSection)
            .AddSingleton<ICacheProvider, DefaultCachingProvider>()
            .AddSingleton<ICachingKeyGenerator, DefaultCachingKeyGenerator>()
            .AddScoped<CachingInterceptor>()
            .AddScoped<CachingAsyncInterceptor>();
    }
}
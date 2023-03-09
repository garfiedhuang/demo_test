using Hkust.Infras.Dapper.Repositories;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddHkustInfraDapper(this IServiceCollection services)
    {
        if (services.HasRegistered(nameof(AddHkustInfraDapper)))
            return services;

        services.TryAddScoped<IAdoExecuterWithQuerierRepository, DapperRepository>();
        services.TryAddScoped<IAdoExecuterRepository, DapperRepository>();
        services.TryAddScoped<IAdoQuerierRepository, DapperRepository>();

        return services;
    }
}
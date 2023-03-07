using Hkust.Infra.Repository.EfCore.MySql;
using Hkust.Infra.Repository.EfCore.MySql.Transaction;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddHkustInfraEfCoreMySql(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsBuilder)
    {
        if (services.HasRegistered(nameof(AddHkustInfraEfCoreMySql)))
            return services;

        services.TryAddScoped<IUnitOfWork, MySqlUnitOfWork<MySqlDbContext>>();
        services.TryAddScoped(typeof(IEfRepository<>), typeof(EfRepository<>));
        services.TryAddScoped(typeof(IEfBasicRepository<>), typeof(EfBasicRepository<>));
        services.AddDbContext<DbContext, MySqlDbContext>(optionsBuilder);

        return services;
    }
}
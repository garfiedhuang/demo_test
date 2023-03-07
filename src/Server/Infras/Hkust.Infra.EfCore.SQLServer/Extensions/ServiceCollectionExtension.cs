using Hkust.Infra.Repository.EfCore.SqlServer;
using Hkust.Infra.Repository.EfCore.SqlServer.Transaction;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddHkustInfraEfCoreSQLServer(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsBuilder)
    {
        if (services.HasRegistered(nameof(AddHkustInfraEfCoreSQLServer)))
            return services;

        services.TryAddScoped<IUnitOfWork, SqlServerUnitOfWork<SqlServerDbContext>>();
        services.TryAddScoped(typeof(IEfRepository<>), typeof(EfRepository<>));
        services.TryAddScoped(typeof(IEfBasicRepository<>), typeof(EfBasicRepository<>));
        services.AddDbContext<DbContext, SqlServerDbContext>(optionsBuilder);

        return services;
    }

    public static IServiceCollection AddHkustInfraEfCoreSQLServer(this IServiceCollection services, IConfigurationSection sqlServerSection)
    {
        var connectionString = sqlServerSection.GetValue<string>("ConnectionString");
        var serviceInfo = services.GetServiceInfo();

        return AddHkustInfraEfCoreSQLServer(services, options =>
        {
            options.UseLowerCaseNamingConvention();
            options.UseSqlServer(connectionString, optionsBuilder =>
            {
                optionsBuilder.MinBatchSize(4)
                                        .MigrationsAssembly(serviceInfo?.StartAssembly?.GetName()?.Name?.Replace("WebApi", "Migrations"))
                                        .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            });

            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (env is not null && env.EqualsIgnoreCase("development"))
            {
                options.LogTo(Console.WriteLine, LogLevel.Information)
                            .EnableSensitiveDataLogging()
                            .EnableDetailedErrors();
            }
        });
    }
}
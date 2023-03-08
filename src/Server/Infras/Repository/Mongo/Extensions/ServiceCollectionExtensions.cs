using Hkust.Infras.IRepositories;
using Hkust.Infras.Repository.Mongo.Configuration;
using Hkust.Infras.Repository.Mongo.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Hkust.Infras.Repository.Mongo.Extensions
{
    /// <summary>
    /// Extensions for <see cref="IServiceCollection"/> to add easy MongoDB wiring.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers the MongoDB context with the specified service collection.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configurator">The configurator.</param>
        /// <returns></returns>
        /// <remarks>
        /// This currently requires wiring up memory caching and logging.
        /// </remarks>
        public static MongoConfigurationBuilder AddHkustInfraMongo<TContext>(this IServiceCollection services, Action<MongoRepositoryOptions> configurator)
            where TContext : IMongoContext
        {
            if (services.HasRegistered(nameof(AddHkustInfraMongo)))
                return default;

            services.Configure(configurator);
            services.AddSingleton(typeof(IMongoContext), typeof(TContext));
            services.AddTransient(typeof(IMongoRepository<>), typeof(MongoRepository<>));
            services.AddTransient(typeof(ISoftDeletableMongoRepository<>), typeof(SoftDeletableMongoRepository<>));
            return new MongoConfigurationBuilder(services);
        }

        /// <summary>
        /// Registers the MongoDB context with the specified service collection.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        /// <remarks>
        /// This currently requires wiring up memory caching and logging.
        /// </remarks>
        public static MongoConfigurationBuilder AddHkustInfraMongo<TContext>(this IServiceCollection services, string connectionString)
             where TContext : IMongoContext
        {
            return services.AddHkustInfraMongo<TContext>(c => c.ConnectionString = connectionString);
        }
    }
}
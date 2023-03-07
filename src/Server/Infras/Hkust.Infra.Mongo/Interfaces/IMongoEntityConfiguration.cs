using Hkust.Infra.Entities;
using Hkust.Infra.Repository.Mongo.Configuration;

namespace Hkust.Infra.Repository.Mongo.Interfaces
{
    /// <summary>
    /// Mongo entity configuration.
    /// </summary>
    public interface IMongoEntityConfiguration<TEntity>
        where TEntity : MongoEntity
    {
        void Configure(MongoEntityBuilder<TEntity> context);
    }
}
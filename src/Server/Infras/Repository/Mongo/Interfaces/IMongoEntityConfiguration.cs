using Hkust.Infras.Entities;
using Hkust.Infras.Repository.Mongo.Configuration;

namespace Hkust.Infras.Repository.Mongo.Interfaces
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
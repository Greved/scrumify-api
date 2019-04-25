using MongoDB.Driver;

namespace Scrumify.DataAccess.Mongo
{
    public interface IMongoCollectionProvider<TEntity>
    {
        IMongoCollection<TEntity> GetCollection();
    }
}
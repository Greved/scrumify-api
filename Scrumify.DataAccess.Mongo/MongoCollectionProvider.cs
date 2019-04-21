using MongoDB.Driver;

namespace Scrumify.DataAccess.Mongo
{
    public class MongoCollectionProvider<TEntity> : IMongoCollectionProvider<TEntity>
    {
        private readonly IMongoStorage mongoStorage;

        public MongoCollectionProvider(IMongoStorage mongoStorage)
        {
            this.mongoStorage = mongoStorage;
        }
        public IMongoCollection<TEntity> GetCollection()
        {
            return mongoStorage.GetCollection<TEntity>(typeof(TEntity).Name);
        }
    }
}
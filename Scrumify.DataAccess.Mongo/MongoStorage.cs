using MongoDB.Driver;

namespace Scrumify.DataAccess.Mongo
{
    public class MongoStorage : IMongoStorage
    {
        private readonly IMongoDatabase database;

        public MongoStorage(IMongoSettings settings)
        {
            var client = new MongoClient(settings.MongoConnectionString);
            database = client.GetDatabase(settings.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return database.GetCollection<T>(name);
        }
    }
}
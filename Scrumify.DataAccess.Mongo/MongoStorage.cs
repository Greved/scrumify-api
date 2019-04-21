using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Scrumify.DataAccess.Mongo
{
    public class MongoStorage : IMongoStorage
    {
        private readonly IMongoDatabase database;

        public MongoStorage(IOptions<MongoSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return database.GetCollection<T>(name);
        }
    }
}
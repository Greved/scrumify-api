using MongoDB.Driver;

namespace Scrumify.DataAccess.Mongo
{
    public interface IMongoStorage
    {
        IMongoCollection<T> GetCollection<T>(string name);
    }
}
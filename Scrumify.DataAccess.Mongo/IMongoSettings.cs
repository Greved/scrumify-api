namespace Scrumify.DataAccess.Mongo
{
    public interface IMongoSettings
    {
        string MongoConnectionString { get; }
        string DatabaseName { get; }
    }
}
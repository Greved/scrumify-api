namespace Scrumify.DataAccess.Mongo.Tests
{
    public class MongoTestSettings: IMongoSettings
    {
        public string MongoConnectionString { get; } = "mongodb://localhost";
        public string DatabaseName { get; } = "scrumify-test";
    }
}
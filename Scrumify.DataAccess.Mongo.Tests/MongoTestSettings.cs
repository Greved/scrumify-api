using Microsoft.Extensions.Options;

namespace Scrumify.DataAccess.Mongo.Tests
{
    public class MongoTestSettingsOptions : IOptions<MongoSettings>
    {
        public MongoSettings Value { get; } = new MongoSettings
        {
            ConnectionString = "mongodb://localhost",
            Database = "scrumify-test"
        };
    }
}
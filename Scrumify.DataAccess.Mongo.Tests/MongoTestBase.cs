using NUnit.Framework;

namespace Scrumify.DataAccess.Mongo.Tests
{
    [TestFixture]
    public class MongoTestBase
    {
        protected IMongoStorage MongoStorage;

        [OneTimeSetUp]
        public virtual void Setup()
        {
            MongoStorage = new MongoStorage(new MongoTestSettings());
        }
    }
}
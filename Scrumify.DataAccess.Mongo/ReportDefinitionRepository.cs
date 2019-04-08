using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Scrumify.DataAccess.Models;
using Serilog;

namespace Scrumify.DataAccess.Mongo
{
    public class ReportDefinitionRepository: IReportDefinitionRepository
    {
        private readonly IMongoStorage mongoStorage;

        public ReportDefinitionRepository(IMongoStorage mongoStorage)
        {
            this.mongoStorage = mongoStorage;
        }

        public async Task<string> SaveAsync(ReportDefinition definition)
        {
            try
            {
                if (string.IsNullOrEmpty(definition.Id))
                {
                    definition.Id = ObjectId.GenerateNewId().ToString();
                }

                var replaceOneResult = await GetCollection()
                    .ReplaceOneAsync(n => n.Id.Equals(definition.Id)
                        , definition
                        , new UpdateOptions { IsUpsert = true });

                return replaceOneResult.UpsertedId?.AsNullableObjectId?.ToString();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Save of definition with {DefinitionId} failed", definition.Id);
                throw;
            }
        }

        public async Task<ReportDefinition> ReadAsync(string id)
        {
            try
            {
                var filter = Builders<ReportDefinition>.Filter.Eq(s => s.Id, id);
                var result = await GetCollection()
                    .Find(filter)
                    .FirstOrDefaultAsync();
                return result;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Read definition by {DefinitionId} failed", id);
                throw;
            }
        }

        public async Task<long> DeleteAllAsync()
        {
            try
            {
                var deleteResult = await GetCollection().DeleteManyAsync(new BsonDocument());
                return deleteResult.DeletedCount;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Delete all reportDefinitions failed");
                throw;
            }
        }

        private IMongoCollection<ReportDefinition> GetCollection()
        {
            return mongoStorage.GetCollection<ReportDefinition>("ReportDefinition");
        }
    }
}
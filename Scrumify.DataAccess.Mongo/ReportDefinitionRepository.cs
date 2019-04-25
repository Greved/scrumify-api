using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Scrumify.DataAccess.Models;
using Serilog;

namespace Scrumify.DataAccess.Mongo
{
    //TODO: create common mongo repository with save, read, delete all methods for generic entity?
    public class ReportDefinitionRepository: IReportDefinitionRepository
    {
        private readonly IMongoCollectionProvider<ReportDefinition> mongoCollectionProvider;
        private readonly IMongoStorage mongoStorage;

        public ReportDefinitionRepository(IMongoCollectionProvider<ReportDefinition> mongoCollectionProvider, IMongoStorage mongoStorage)
        {
            this.mongoCollectionProvider = mongoCollectionProvider;
            this.mongoStorage = mongoStorage;
        }

        public async Task<IList<ReportDefinitionListItem>> ReadAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var projection = Builders<ReportDefinitionListItem>.Projection
                    .Include(x => x.Id)
                    .Include(x => x.Name);
                var result = await mongoStorage.GetCollection<ReportDefinitionListItem>(nameof(ReportDefinition))
                    .Find(new BsonDocument())
                    .Project<ReportDefinitionListItem>(projection)
                    .ToListAsync(cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Read all of definitions failed");
                throw;
            }
        }

        public async Task<string> SaveAsync(ReportDefinition definition, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                if (string.IsNullOrEmpty(definition.Id))
                {
                    definition.Id = ObjectId.GenerateNewId().ToString();
                }

                var replaceOneResult = await mongoCollectionProvider.GetCollection()
                    .ReplaceOneAsync(n => n.Id.Equals(definition.Id)
                        , definition
                        , new UpdateOptions { IsUpsert = true }
                        , cancellationToken);

                return replaceOneResult.UpsertedId?.AsNullableObjectId?.ToString();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Save of definition with {DefinitionId} failed", definition.Id);
                throw;
            }
        }

        public async Task<ReportDefinition> ReadAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var filter = Builders<ReportDefinition>.Filter.Eq(s => s.Id, id);
                var result = await mongoCollectionProvider.GetCollection()
                    .Find(filter)
                    .FirstOrDefaultAsync(cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Read definition by {DefinitionId} failed", id);
                throw;
            }
        }

        public async Task<long> DeleteAllAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var deleteResult = await mongoCollectionProvider.GetCollection().DeleteManyAsync(new BsonDocument(), cancellationToken);
                return deleteResult.DeletedCount;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Delete all reportDefinitions failed");
                throw;
            }
        }
    }
}
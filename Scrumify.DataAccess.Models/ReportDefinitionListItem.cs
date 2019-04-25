using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Scrumify.DataAccess.Models
{
    public class ReportDefinitionListItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
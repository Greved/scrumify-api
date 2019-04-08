using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Scrumify.DataAccess.Models
{
    public class ReportDefinitionQuestionOption
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Text { get; set; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Text)}: {Text}";
        }
    }
}
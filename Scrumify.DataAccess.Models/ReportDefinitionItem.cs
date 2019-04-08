using MongoDB.Bson.Serialization.Attributes;

namespace Scrumify.DataAccess.Models
{
    public class ReportDefinitionItem
    {
        public int Order { get; set; }

        [BsonIgnoreIfNull]
        public ReportDefinitionQuestionGroup Group { get; set; }

        [BsonIgnoreIfNull]
        public ReportDefinitionQuestion Question { get; set; }
    }
}
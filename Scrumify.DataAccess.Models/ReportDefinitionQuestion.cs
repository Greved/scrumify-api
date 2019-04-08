using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Scrumify.DataAccess.Models
{
    public class ReportDefinitionQuestion
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Text { get; set; }

        public ReportDefinitionQuestionType Type { get; set; }

        [BsonIgnoreIfNull]
        public List<ReportDefinitionQuestionOption> Options { get; set; }

        //TODO: IssueTracker task question type

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Text)}: {Text}, {nameof(Type)}: {Type}";
        }
    }
}
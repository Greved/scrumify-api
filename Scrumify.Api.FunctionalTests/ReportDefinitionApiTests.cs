using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using MongoDB.Bson;
using NUnit.Framework;
using Scrumify.Api.Client.Models.ReportDefinition;
using Scrumify.Api.Client.Models.ReportDefinition.List;

namespace Scrumify.Api.FunctionalTests
{
    [TestFixture]
    public class ReportDefinitionApiTests: ScrumifyApiTestBase
    {
        [Test]
        public async Task Get_Should_Return_Saved_Definitions()
        {
            var definition = new ReportDefinitionDto
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "some test definition1",
                Items = new List<ReportDefinitionItemDto>
                {
                    new ReportDefinitionItemDto{Order = 1, Question = new ReportDefinitionQuestionDto
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        Type = ReportDefinitionQuestionTypeDto.Text,
                        Text = "some test question1"
                    }}
                }
            };
            await ApiLocal.Client.ReportDefinition.SaveAsync(definition);

            var allDefinitions = await ApiLocal.Client.ReportDefinition.GetAsync();
            var expected = new List<ReportDefinitionListItemDto>{new ReportDefinitionListItemDto{Id = definition.Id, Name = definition.Name}};
            allDefinitions.Should().BeEquivalentTo(expected);
        }

        [TearDown]
        public async Task TestTearDown()
        {
            await ApiLocal.Client.ReportDefinition.DeleteAllAsync();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using MongoDB.Bson;
using NUnit.Framework;
using Scrumify.Api.Client.Exceptions;
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

        [Test]
        public async Task Get_Should_Return_Empty_Definitions_List_If_There_Are_No_Definitions()
        {
            var allDefinitions = await ApiLocal.Client.ReportDefinition.GetAsync();
            allDefinitions.Should().BeEmpty();
        }

        [TestCaseSource(nameof(Save_Invalid_Definition_Should_Return_Validation_Error_Cases))]
        public void Save_Invalid_Definition_Should_Return_Validation_Error(ReportDefinitionDto definition)
        {
            Func<Task> saveAction = async () => await ApiLocal.Client.ReportDefinition.SaveAsync(definition);
            saveAction.Should().Throw<ScrumifyApiClientValidationException>();
        }

        static TestCaseData[] Save_Invalid_Definition_Should_Return_Validation_Error_Cases = 
            {new TestCaseData(new ReportDefinitionDto
                {
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
                }).SetName("Invalid definition. No Id"),
                new TestCaseData(new ReportDefinitionDto
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Items = new List<ReportDefinitionItemDto>
                    {
                        new ReportDefinitionItemDto{Order = 1, Question = new ReportDefinitionQuestionDto
                        {
                            Id = ObjectId.GenerateNewId().ToString(),
                            Type = ReportDefinitionQuestionTypeDto.Text,
                            Text = "some test question1"
                        }}
                    }
                }).SetName("Invalid definition. No Name"),
                new TestCaseData(new ReportDefinitionDto
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Items = new List<ReportDefinitionItemDto>()
                }).SetName("Invalid definition. No Items"),
                new TestCaseData(null).SetName("Invalid definition. Null definition")
            };

        [TearDown]
        public async Task TestTearDown()
        {
            await ApiLocal.Client.ReportDefinition.DeleteAllAsync();
        }
    }
}
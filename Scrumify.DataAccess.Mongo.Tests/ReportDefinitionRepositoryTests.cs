using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using MongoDB.Bson;
using NUnit.Framework;
using Scrumify.DataAccess.Models;

namespace Scrumify.DataAccess.Mongo.Tests
{
    [TestFixture]
    public class ReportDefinitionRepositoryTests: MongoTestBase
    {
        private ReportDefinitionRepository repository;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            repository = new ReportDefinitionRepository(MongoStorage);
        }

        [TearDown]
        public async Task TearDown()
        {
            await repository.DeleteAllAsync();
        }

        [Test]
        public async Task Read_After_Write_Should_Return_The_Same_Object()
        {
            var reportDefinition = new ReportDefinition
            {
                Name = "name1",
                Items = new[]
                {
                    new ReportDefinitionItem
                    {
                        Order = 2,
                        Question = new ReportDefinitionQuestion
                        {
                            Id = ObjectId.GenerateNewId().ToString(),
                            Text = "some text1",
                            Type = ReportDefinitionQuestionType.Text
                        }
                    },
                    new ReportDefinitionItem
                    {
                        Order = 3,
                        Question = new ReportDefinitionQuestion
                        {
                            Id = ObjectId.GenerateNewId().ToString(),
                            Text = "some text2",
                            Type = ReportDefinitionQuestionType.Poll,
                            Options = new List<ReportDefinitionQuestionOption>
                            {
                                new ReportDefinitionQuestionOption
                                {
                                    Id = ObjectId.GenerateNewId().ToString(),
                                    Text = "option1"
                                },
                                new ReportDefinitionQuestionOption
                                {
                                    Id = ObjectId.GenerateNewId().ToString(),
                                    Text = "option2"
                                }
                            }
                        }
                    },
                    new ReportDefinitionItem
                    {
                        Order = 4,
                        Group = new ReportDefinitionQuestionGroup
                        {
                            Id = ObjectId.GenerateNewId().ToString(),
                            Name = "group1",
                            Questions = new List<ReportDefinitionQuestion>
                            {
                                new ReportDefinitionQuestion
                                {
                                    Id = ObjectId.GenerateNewId().ToString(),
                                    Text = "some group question1",
                                    Type = ReportDefinitionQuestionType.Text
                                },
                                new ReportDefinitionQuestion
                                {
                                    Id = ObjectId.GenerateNewId().ToString(),
                                    Text = "some group question2",
                                    Type = ReportDefinitionQuestionType.Text
                                },
                                new ReportDefinitionQuestion
                                {
                                    Id = ObjectId.GenerateNewId().ToString(),
                                    Text = "some group question3",
                                    Type = ReportDefinitionQuestionType.Poll,
                                    Options = new List<ReportDefinitionQuestionOption>
                                    {
                                        new ReportDefinitionQuestionOption
                                        {
                                            Id = ObjectId.GenerateNewId().ToString(),
                                            Text = "option3"
                                        },
                                        new ReportDefinitionQuestionOption
                                        {
                                            Id = ObjectId.GenerateNewId().ToString(),
                                            Text = "option4"
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
            var id = await repository.SaveAsync(reportDefinition);

            var storedDefinition = await repository.ReadAsync(id);
            storedDefinition.Should().BeEquivalentTo(reportDefinition);
        }

        [Test]
        public async Task Read_Absent_Definition_Should_Return_Null()
        {
            var actual = await repository.ReadAsync(ObjectId.GenerateNewId().ToString());
            actual.Should().BeNull();
        }

        [Test]
        public async Task Save_Existing_Definition_Should_Update_Id()
        {
            var reportDefinition = new ReportDefinition
            {
                Name = "name1"
            };
            await repository.SaveAsync(reportDefinition);

            reportDefinition.Name = "name2";
            await repository.SaveAsync(reportDefinition);

            var storedDefinition = await repository.ReadAsync(reportDefinition.Id);
            storedDefinition.Should().BeEquivalentTo(reportDefinition);
        }

        [Test]
        public async Task Save_Should_Generate_And_Return_Id_For_Created_Definition()
        {
            var reportDefinition = new ReportDefinition
            {
                Name = "name1"
            };
            var id = await repository.SaveAsync(reportDefinition);
            reportDefinition.Id.Should().Be(id);
        }

        [Test]
        public async Task Save_Should_Return_Null_When_Definition_Was_Updated()
        {
            var reportDefinition = new ReportDefinition
            {
                Name = "name1"
            };
            await repository.SaveAsync(reportDefinition);
            reportDefinition.Items = new[] {new ReportDefinitionItem{Order = 2}};
            var actual = await repository.SaveAsync(reportDefinition);
            actual.Should().BeNull();
        }
    }
}
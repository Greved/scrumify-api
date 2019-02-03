using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Greved.Core;
using NUnit.Framework;
using Scrumify.DataAccess.Core;
using Scrumify.DataAccess.ReportTaskSupport;
using Scrumify.DataAccess.TestCore;
using Scrumify.Models.ReportItem;

namespace Scrumify.DataAccess.Tests
{
    [TestFixture]
    public class ReportTaskRepositoryTests: DbTestBase
    {
        private ReportTaskRepository repository;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            var queryExecuter = new QueryExecuter(DbConnectionStringProvider);
            repository = new ReportTaskRepository(queryExecuter);
        }

        [TearDown]
        public async Task TearDown()
        {
            await repository.DeleteAllAsync();
        }

        [Test]
        public async Task ReadByDateAndUserAsync_Should_Return_Tasks_With_Date_Between_Start_And_End_Of_Specified_Day()
        {
            var teamId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var reportDate = new DateTime(2019, 1, 14);
            var reportDateItemIdentifier = new ReportDateItemIdentifier
            {
                TeamId = teamId,
                Date = reportDate,
                UserId = userId
            };
            var reportTask1 = new ReportTask
            {
                Id = Guid.NewGuid(),
                CurrentState = "some state",
                Problems = "some problems",
                Url = "some url",
                Theme = "some theme",
                IsPublic = true,
                DateItemIdentifier = reportDateItemIdentifier
            };
            var reportTask2 = new ReportTask
            {
                Id = Guid.NewGuid(),
                CurrentState = "some state2",
                Problems = "some problems2",
                Url = "some url2",
                Theme = "some theme2",
                IsPublic = true,
                DateItemIdentifier = reportDateItemIdentifier
            };
            var reportTask3 = new ReportTask
            {
                Id = Guid.NewGuid(),
                CurrentState = "some state3",
                Problems = "some problems3",
                Url = "some url3",
                Theme = "some theme3",
                IsPublic = true,
                DateItemIdentifier = new ReportDateItemIdentifier
                {
                    Date = reportDateItemIdentifier.Date.AddDays(2),
                    TeamId = reportDateItemIdentifier.TeamId,
                    UserId = reportDateItemIdentifier.UserId
                }
            };
            var reportTask4 = new ReportTask
            {
                Id = Guid.NewGuid(),
                CurrentState = "some state4",
                Problems = "some problems4",
                Url = "some url4",
                Theme = "some theme4",
                IsPublic = true,
                DateItemIdentifier = new ReportDateItemIdentifier
                {
                    Date = reportDateItemIdentifier.Date.AddDays(-2),
                    TeamId = reportDateItemIdentifier.TeamId,
                    UserId = reportDateItemIdentifier.UserId
                }
            };

            await repository.SaveAsync(reportTask1).ConfigureAwait(false);
            await repository.SaveAsync(reportTask3).ConfigureAwait(false);
            await repository.SaveAsync(reportTask2).ConfigureAwait(false);
            await repository.SaveAsync(reportTask4).ConfigureAwait(false);

            var readedTask = await repository.ReadByDateAndUserAsync(reportDateItemIdentifier, true).ConfigureAwait(false);
            readedTask.Should().BeEquivalentTo(new List<ReportTask>{reportTask1, reportTask2});
        }

        [Test]
        public async Task ReadByDateAndUserAsync_Should_Return_Exactly_Saved_Task()
        {
            var teamId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var reportDate = new DateTime(2019, 1, 14);
            var reportDateItemIdentifier = new ReportDateItemIdentifier
            {
                TeamId = teamId,
                Date = reportDate,
                UserId = userId
            };
            var reportTask = new ReportTask
            {
                Id = Guid.NewGuid(),
                CurrentState = "some state",
                Problems = "some problems",
                Url = "some url",
                Theme = "some theme",
                IsPublic = true,
                DateItemIdentifier = reportDateItemIdentifier
            };

            await repository.SaveAsync(reportTask).ConfigureAwait(false);

            var readedTask = await repository.ReadByDateAndUserAsync(reportDateItemIdentifier, true).ConfigureAwait(false);
            readedTask.Should().BeEquivalentTo(reportTask);
        }

        [Test]
        public async Task ReadByDateAndUserAsync_Should_Filter_By_UserId()
        {
            var teamId = Guid.NewGuid();
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            var reportDate = new DateTime(2019, 1, 14);
            var reportDateItemIdentifier = new ReportDateItemIdentifier
            {
                TeamId = teamId,
                Date = reportDate,
                UserId = userId1
            };
            var reportTask1 = new ReportTask
            {
                Id = Guid.NewGuid(),
                CurrentState = "some state",
                Problems = "some problems",
                Url = "some url",
                Theme = "some theme",
                IsPublic = true,
                DateItemIdentifier = reportDateItemIdentifier
            };
            var reportTask2 = new ReportTask
            {
                Id = Guid.NewGuid(),
                CurrentState = "some state2",
                Problems = "some problems2",
                Url = "some url2",
                Theme = "some theme2",
                IsPublic = true,
                DateItemIdentifier = new ReportDateItemIdentifier
                {
                    Date = reportDateItemIdentifier.Date,
                    UserId = userId2,
                    TeamId = reportDateItemIdentifier.TeamId
                }
            };
            var reportTask3 = new ReportTask
            {
                Id = Guid.NewGuid(),
                CurrentState = "some state3",
                Problems = "some problems",
                Url = "some url3",
                Theme = "some theme3",
                IsPublic = true,
                DateItemIdentifier = reportDateItemIdentifier
            };

            await repository.SaveAsync(reportTask1).ConfigureAwait(false);
            await repository.SaveAsync(reportTask2).ConfigureAwait(false);
            await repository.SaveAsync(reportTask3).ConfigureAwait(false);

            var readedTask = await repository.ReadByDateAndUserAsync(reportDateItemIdentifier, true).ConfigureAwait(false);
            readedTask.Should().BeEquivalentTo(new List<ReportTask> {reportTask1, reportTask3});
        }

        [Test]
        public async Task ReadByDateAndUserAsync_Should_Filter_By_TeamId()
        {
            var teamId1 = Guid.NewGuid();
            var teamId2 = Guid.NewGuid();
            var reportDate = new DateTime(2019, 1, 14);
            var reportDateItemIdentifier = new ReportDateItemIdentifier
            {
                TeamId = teamId1,
                Date = reportDate,
                UserId = Guid.NewGuid()
            };
            var reportTask1 = new ReportTask
            {
                Id = Guid.NewGuid(),
                CurrentState = "some state",
                Problems = "some problems",
                Url = "some url",
                Theme = "some theme",
                IsPublic = true,
                DateItemIdentifier = reportDateItemIdentifier
            };
            var reportTask2 = new ReportTask
            {
                Id = Guid.NewGuid(),
                CurrentState = "some state2",
                Problems = "some problems2",
                Url = "some url2",
                Theme = "some theme2",
                IsPublic = true,
                DateItemIdentifier = new ReportDateItemIdentifier
                {
                    Date = reportDateItemIdentifier.Date,
                    UserId = reportDateItemIdentifier.UserId,
                    TeamId = teamId2
                }
            };
            var reportTask3 = new ReportTask
            {
                Id = Guid.NewGuid(),
                CurrentState = "some state3",
                Problems = "some problems",
                Url = "some url3",
                Theme = "some theme3",
                IsPublic = true,
                DateItemIdentifier = reportDateItemIdentifier
            };

            await repository.SaveAsync(reportTask1).ConfigureAwait(false);
            await repository.SaveAsync(reportTask2).ConfigureAwait(false);
            await repository.SaveAsync(reportTask3).ConfigureAwait(false);

            var readedTask = await repository.ReadByDateAndUserAsync(reportDateItemIdentifier, true).ConfigureAwait(false);
            readedTask.Should().BeEquivalentTo(new List<ReportTask> { reportTask1, reportTask3 });
        }

        [Test]
        public async Task ReadByDateAndUserAsync_Should_Filter_By_IsPublic_Flag()
        {
            var reportDateItemIdentifier = new ReportDateItemIdentifier
            {
                TeamId = Guid.NewGuid(),
                Date = new DateTime(2019, 1, 14),
                UserId = Guid.NewGuid()
            };
            var reportTask1 = new ReportTask
            {
                Id = Guid.NewGuid(),
                CurrentState = "some state",
                Problems = "some problems",
                Url = "some url",
                Theme = "some theme",
                IsPublic = true,
                DateItemIdentifier = reportDateItemIdentifier
            };
            var reportTask2 = new ReportTask
            {
                Id = Guid.NewGuid(),
                CurrentState = "some state2",
                Problems = "some problems2",
                Url = "some url2",
                Theme = "some theme2",
                IsPublic = false,
                DateItemIdentifier = reportDateItemIdentifier
            };
            var reportTask3 = new ReportTask
            {
                Id = Guid.NewGuid(),
                CurrentState = "some state3",
                Problems = "some problems",
                Url = "some url3",
                Theme = "some theme3",
                IsPublic = true,
                DateItemIdentifier = reportDateItemIdentifier
            };

            await repository.SaveAsync(reportTask1).ConfigureAwait(false);
            await repository.SaveAsync(reportTask2).ConfigureAwait(false);
            await repository.SaveAsync(reportTask3).ConfigureAwait(false);

            var readedTask = await repository.ReadByDateAndUserAsync(reportDateItemIdentifier, true).ConfigureAwait(false);
            readedTask.Should().BeEquivalentTo(new List<ReportTask> { reportTask1, reportTask3 });
        }

        [Test]
        public async Task ReadByDateAndUserAsync_Should_Return_Empty_List_If_There_Are_No_Suitable_Tasks()
        {
            var reportDateItemIdentifier = new ReportDateItemIdentifier
            {
                TeamId = Guid.NewGuid(),
                Date = new DateTime(2019, 1, 14),
                UserId = Guid.NewGuid()
            };
            var reportTask1 = new ReportTask
            {
                Id = Guid.NewGuid(),
                CurrentState = "some state",
                Problems = "some problems",
                Url = "some url",
                Theme = "some theme",
                IsPublic = true,
                DateItemIdentifier = reportDateItemIdentifier
            };
            var reportTask2 = new ReportTask
            {
                Id = Guid.NewGuid(),
                CurrentState = "some state2",
                Problems = "some problems2",
                Url = "some url2",
                Theme = "some theme2",
                IsPublic = false,
                DateItemIdentifier = reportDateItemIdentifier
            };

            await repository.SaveAsync(reportTask1).ConfigureAwait(false);
            await repository.SaveAsync(reportTask2).ConfigureAwait(false);

            var readedTask = await repository.ReadByDateAndUserAsync(new ReportDateItemIdentifier
            {
                Date = DateTime.Now,
                UserId = Guid.NewGuid(),
                TeamId = Guid.NewGuid()
            }, true).ConfigureAwait(false);
            readedTask.Should().BeEquivalentTo(EmptyCollection<ReportTask>.List);
        }

        [Test]
        public async Task ReadByDateAndUserAsync_Should_Return_Empty_List_If_There_Are_No_Tasks()
        {
            var readedTask = await repository.ReadByDateAndUserAsync(new ReportDateItemIdentifier
            {
                Date = DateTime.Now,
                UserId = Guid.NewGuid(),
                TeamId = Guid.NewGuid()
            }, true).ConfigureAwait(false);
            readedTask.Should().BeEquivalentTo(EmptyCollection<ReportTask>.List);
        }

        [Test]
        public async Task ReadByDateAsync_Should_Return_Tasks_With_Date_Between_Start_And_End_Of_Specified_Day()
        {
            var teamId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var reportDate = new DateTime(2019, 1, 14);
            var reportDateItemIdentifier = new ReportDateItemIdentifier
            {
                TeamId = teamId,
                Date = reportDate,
                UserId = userId
            };
            var reportTask1 = new ReportTask
            {
                Id = Guid.NewGuid(),
                CurrentState = "some state",
                Problems = "some problems",
                Url = "some url",
                Theme = "some theme",
                IsPublic = true,
                DateItemIdentifier = reportDateItemIdentifier
            };
            var reportTask2 = new ReportTask
            {
                Id = Guid.NewGuid(),
                CurrentState = "some state2",
                Problems = "some problems2",
                Url = "some url2",
                Theme = "some theme2",
                IsPublic = true,
                DateItemIdentifier = reportDateItemIdentifier
            };
            var reportTask3 = new ReportTask
            {
                Id = Guid.NewGuid(),
                CurrentState = "some state3",
                Problems = "some problems3",
                Url = "some url3",
                Theme = "some theme3",
                IsPublic = true,
                DateItemIdentifier = new ReportDateItemIdentifier
                {
                    Date = reportDateItemIdentifier.Date.AddDays(2),
                    TeamId = reportDateItemIdentifier.TeamId,
                    UserId = reportDateItemIdentifier.UserId
                }
            };
            var reportTask4 = new ReportTask
            {
                Id = Guid.NewGuid(),
                CurrentState = "some state4",
                Problems = "some problems4",
                Url = "some url4",
                Theme = "some theme4",
                IsPublic = true,
                DateItemIdentifier = new ReportDateItemIdentifier
                {
                    Date = reportDateItemIdentifier.Date.AddDays(-2),
                    TeamId = reportDateItemIdentifier.TeamId,
                    UserId = reportDateItemIdentifier.UserId
                }
            };

            await repository.SaveAsync(reportTask1).ConfigureAwait(false);
            await repository.SaveAsync(reportTask3).ConfigureAwait(false);
            await repository.SaveAsync(reportTask2).ConfigureAwait(false);
            await repository.SaveAsync(reportTask4).ConfigureAwait(false);

            var readedTask = await repository.ReadByDateAsync(teamId, reportDate, true).ConfigureAwait(false);
            readedTask.Should().BeEquivalentTo(new List<ReportTask> { reportTask1, reportTask2 });
        }

        [Test]
        public async Task ReadByDateAsync_Should_Return_Tasks_Of_All_Users_In_Team()
        {
            var teamId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var reportDate = new DateTime(2019, 1, 14);
            var reportDateItemIdentifier = new ReportDateItemIdentifier
            {
                TeamId = teamId,
                Date = reportDate,
                UserId = userId
            };
            var reportTask1 = new ReportTask
            {
                Id = Guid.NewGuid(),
                CurrentState = "some state",
                Problems = "some problems",
                Url = "some url",
                Theme = "some theme",
                IsPublic = true,
                DateItemIdentifier = reportDateItemIdentifier
            };
            var reportTask2 = new ReportTask
            {
                Id = Guid.NewGuid(),
                CurrentState = "some state2",
                Problems = "some problems2",
                Url = "some url2",
                Theme = "some theme2",
                IsPublic = true,
                DateItemIdentifier = reportDateItemIdentifier
            };
            var reportTask3 = new ReportTask
            {
                Id = Guid.NewGuid(),
                CurrentState = "some state3",
                Problems = "some problems3",
                Url = "some url3",
                Theme = "some theme3",
                IsPublic = true,
                DateItemIdentifier = new ReportDateItemIdentifier
                {
                    Date = reportDateItemIdentifier.Date.AddDays(2),
                    TeamId = reportDateItemIdentifier.TeamId,
                    UserId = reportDateItemIdentifier.UserId
                }
            };
            var reportTask4 = new ReportTask
            {
                Id = Guid.NewGuid(),
                CurrentState = "some state4",
                Problems = "some problems4",
                Url = "some url4",
                Theme = "some theme4",
                IsPublic = true,
                DateItemIdentifier = new ReportDateItemIdentifier
                {
                    Date = reportDateItemIdentifier.Date,
                    TeamId = reportDateItemIdentifier.TeamId,
                    UserId = Guid.NewGuid()
                }
            };

            await repository.SaveAsync(reportTask1).ConfigureAwait(false);
            await repository.SaveAsync(reportTask3).ConfigureAwait(false);
            await repository.SaveAsync(reportTask2).ConfigureAwait(false);
            await repository.SaveAsync(reportTask4).ConfigureAwait(false);

            var readedTask = await repository.ReadByDateAsync(teamId, reportDate, true).ConfigureAwait(false);
            readedTask.Should().BeEquivalentTo(new List<ReportTask> { reportTask1, reportTask2, reportTask4 });
        }

        [Test]
        public async Task ReadByDateAsync_Should_Filter_By_TeamId()
        {
            var teamId1 = Guid.NewGuid();
            var teamId2 = Guid.NewGuid();
            var reportDate = new DateTime(2019, 1, 14);
            var reportDateItemIdentifier = new ReportDateItemIdentifier
            {
                TeamId = teamId1,
                Date = reportDate,
                UserId = Guid.NewGuid()
            };
            var reportTask1 = new ReportTask
            {
                Id = Guid.NewGuid(),
                CurrentState = "some state",
                Problems = "some problems",
                Url = "some url",
                Theme = "some theme",
                IsPublic = true,
                DateItemIdentifier = reportDateItemIdentifier
            };
            var reportTask2 = new ReportTask
            {
                Id = Guid.NewGuid(),
                CurrentState = "some state2",
                Problems = "some problems2",
                Url = "some url2",
                Theme = "some theme2",
                IsPublic = true,
                DateItemIdentifier = new ReportDateItemIdentifier
                {
                    Date = reportDateItemIdentifier.Date,
                    UserId = reportDateItemIdentifier.UserId,
                    TeamId = teamId2
                }
            };
            var reportTask3 = new ReportTask
            {
                Id = Guid.NewGuid(),
                CurrentState = "some state3",
                Problems = "some problems",
                Url = "some url3",
                Theme = "some theme3",
                IsPublic = true,
                DateItemIdentifier = reportDateItemIdentifier
            };

            await repository.SaveAsync(reportTask1).ConfigureAwait(false);
            await repository.SaveAsync(reportTask2).ConfigureAwait(false);
            await repository.SaveAsync(reportTask3).ConfigureAwait(false);

            var readedTask = await repository.ReadByDateAsync(teamId1, reportDate, true).ConfigureAwait(false);
            readedTask.Should().BeEquivalentTo(new List<ReportTask> { reportTask1, reportTask3 });
        }

        [Test]
        public async Task ReadByDateAsync_Should_Filter_By_IsPublic_Flag()
        {
            var reportDateItemIdentifier = new ReportDateItemIdentifier
            {
                TeamId = Guid.NewGuid(),
                Date = new DateTime(2019, 1, 14),
                UserId = Guid.NewGuid()
            };
            var reportTask1 = new ReportTask
            {
                Id = Guid.NewGuid(),
                CurrentState = "some state",
                Problems = "some problems",
                Url = "some url",
                Theme = "some theme",
                IsPublic = true,
                DateItemIdentifier = reportDateItemIdentifier
            };
            var reportTask2 = new ReportTask
            {
                Id = Guid.NewGuid(),
                CurrentState = "some state2",
                Problems = "some problems2",
                Url = "some url2",
                Theme = "some theme2",
                IsPublic = false,
                DateItemIdentifier = reportDateItemIdentifier
            };
            var reportTask3 = new ReportTask
            {
                Id = Guid.NewGuid(),
                CurrentState = "some state3",
                Problems = "some problems",
                Url = "some url3",
                Theme = "some theme3",
                IsPublic = true,
                DateItemIdentifier = reportDateItemIdentifier
            };

            await repository.SaveAsync(reportTask1).ConfigureAwait(false);
            await repository.SaveAsync(reportTask2).ConfigureAwait(false);
            await repository.SaveAsync(reportTask3).ConfigureAwait(false);

            var readedTask = await repository.ReadByDateAsync(reportDateItemIdentifier.TeamId, reportDateItemIdentifier.Date, true).ConfigureAwait(false);
            readedTask.Should().BeEquivalentTo(new List<ReportTask> { reportTask1, reportTask3 });
        }

        [Test]
        public async Task ReadByDateAsync_Should_Return_Empty_List_If_There_Are_No_Suitable_Tasks()
        {
            var reportDateItemIdentifier = new ReportDateItemIdentifier
            {
                TeamId = Guid.NewGuid(),
                Date = new DateTime(2019, 1, 14),
                UserId = Guid.NewGuid()
            };
            var reportTask1 = new ReportTask
            {
                Id = Guid.NewGuid(),
                CurrentState = "some state",
                Problems = "some problems",
                Url = "some url",
                Theme = "some theme",
                IsPublic = true,
                DateItemIdentifier = reportDateItemIdentifier
            };
            var reportTask2 = new ReportTask
            {
                Id = Guid.NewGuid(),
                CurrentState = "some state2",
                Problems = "some problems2",
                Url = "some url2",
                Theme = "some theme2",
                IsPublic = false,
                DateItemIdentifier = reportDateItemIdentifier
            };

            await repository.SaveAsync(reportTask1).ConfigureAwait(false);
            await repository.SaveAsync(reportTask2).ConfigureAwait(false);

            var readedTask = await repository.ReadByDateAsync(Guid.NewGuid(), DateTime.Now, true).ConfigureAwait(false);
            readedTask.Should().BeEquivalentTo(EmptyCollection<ReportTask>.List);
        }

        [Test]
        public async Task ReadByDateAsync_Should_Return_Empty_List_If_There_Are_No_Tasks()
        {
            var readedTask = await repository.ReadByDateAsync(Guid.NewGuid(), DateTime.Now, true).ConfigureAwait(false);
            readedTask.Should().BeEquivalentTo(EmptyCollection<ReportTask>.List);
        }

        [Test]
        public async Task DeleteAsync_Should_Delete_Just_Task_With_Specified_Id()
        {
            var reportDateItemIdentifier = new ReportDateItemIdentifier
            {
                TeamId = Guid.NewGuid(),
                Date = new DateTime(2019, 1, 14),
                UserId = Guid.NewGuid()
            };
            var taskId1 = Guid.NewGuid();
            var reportTask1 = new ReportTask
            {
                Id = taskId1,
                CurrentState = "some state",
                Problems = "some problems",
                Url = "some url",
                Theme = "some theme",
                IsPublic = true,
                DateItemIdentifier = reportDateItemIdentifier
            };
            var reportTask2 = new ReportTask
            {
                Id = Guid.NewGuid(),
                CurrentState = "some state2",
                Problems = "some problems2",
                Url = "some url2",
                Theme = "some theme2",
                IsPublic = true,
                DateItemIdentifier = reportDateItemIdentifier
            };

            await repository.SaveAsync(reportTask1).ConfigureAwait(false);
            await repository.SaveAsync(reportTask2).ConfigureAwait(false);

            await repository.DeleteAsync(taskId1).ConfigureAwait(false);
            var readedTask = await repository.ReadByDateAsync(reportDateItemIdentifier.TeamId, reportDateItemIdentifier.Date, true).ConfigureAwait(false);
            readedTask.Should().BeEquivalentTo(new List<ReportTask>{ reportTask2 });
        }

        [Test]
        public async Task DeleteAsync_Should_Do_Nothing_When_Trying_To_Delete_Absent_Task()
        {
            var reportDateItemIdentifier = new ReportDateItemIdentifier
            {
                TeamId = Guid.NewGuid(),
                Date = new DateTime(2019, 1, 14),
                UserId = Guid.NewGuid()
            };
            var taskId1 = Guid.NewGuid();
            var reportTask1 = new ReportTask
            {
                Id = taskId1,
                CurrentState = "some state",
                Problems = "some problems",
                Url = "some url",
                Theme = "some theme",
                IsPublic = true,
                DateItemIdentifier = reportDateItemIdentifier
            };
            var reportTask2 = new ReportTask
            {
                Id = Guid.NewGuid(),
                CurrentState = "some state2",
                Problems = "some problems2",
                Url = "some url2",
                Theme = "some theme2",
                IsPublic = true,
                DateItemIdentifier = reportDateItemIdentifier
            };

            await repository.SaveAsync(reportTask1).ConfigureAwait(false);
            await repository.SaveAsync(reportTask2).ConfigureAwait(false);

            await repository.DeleteAsync(Guid.NewGuid()).ConfigureAwait(false);
            var readedTask = await repository.ReadByDateAsync(reportDateItemIdentifier.TeamId, reportDateItemIdentifier.Date, true).ConfigureAwait(false);
            readedTask.Should().BeEquivalentTo(new List<ReportTask> { reportTask1, reportTask2 });
        }

        [Test]
        public async Task DeleteAsync_Should_Do_Nothing_When_There_Are_No_Tasks()
        {
            await repository.DeleteAsync(Guid.NewGuid()).ConfigureAwait(false);
            var readedTask = await repository.ReadByDateAsync(Guid.NewGuid(), DateTime.Now, true).ConfigureAwait(false);
            readedTask.Should().BeEquivalentTo(EmptyCollection<ReportTask>.List);
        }
    }
}
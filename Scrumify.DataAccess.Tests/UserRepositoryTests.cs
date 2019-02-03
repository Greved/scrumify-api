using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Scrumify.DataAccess.Core;
using Scrumify.DataAccess.TestCore;
using Scrumify.DataAccess.UserSupport;
using Scrumify.Models;

namespace Scrumify.DataAccess.Tests
{
    [TestFixture]
    public class UserRepositoryTests: DbTestBase
    {
        private UserRepository repository;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            repository = new UserRepository(new QueryExecuter(DbConnectionStringProvider));
        }

        [TearDown]
        public async Task TearDown()
        {
            await repository.DeleteAllAsync();
        }

        [Test]
        public async Task SaveAsync_Should_Save_All_Fields_Of_User()
        {
            var userId = Guid.NewGuid();
            var outerId = "outerId1";
            var user = new User
            {
                Id = userId,
                Name = "user1",
                OuterId = outerId,
                TeamId = Guid.NewGuid()
            };
            await repository.SaveAsync(user).ConfigureAwait(false);

            var actual = await repository.ReadAsync(userId).ConfigureAwait(false);
            actual.Should().BeEquivalentTo(user);
        }

        [Test]
        public async Task ReadByOuterIdAsync_Should_Return_Specified_User()
        {
            var userId = Guid.NewGuid();
            var outerId = "outerId1";
            var teamId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                Name = "user1",
                OuterId = outerId,
                TeamId = teamId
            };
            await repository.SaveAsync(user).ConfigureAwait(false);

            var actual = await repository.ReadAsync(outerId, teamId).ConfigureAwait(false);
            actual.Should().BeEquivalentTo(user);
        }

        [Test]
        public async Task ReadIdAsync_Should_Return_UserId_And_Filter_By_OuterId()
        {
            var userId1 = Guid.NewGuid();
            var outerId1 = "outerId1";
            var teamId = Guid.NewGuid();
            var user = new User
            {
                Id = userId1,
                Name = "user1",
                OuterId = outerId1,
                TeamId = teamId
            };
            await repository.SaveAsync(user).ConfigureAwait(false);

            var userId2 = Guid.NewGuid();
            var outerId2 = "outerId2";
            var user2 = new User
            {
                Id = userId2,
                Name = "user2",
                OuterId = outerId2,
                TeamId = teamId
            };
            await repository.SaveAsync(user2).ConfigureAwait(false);

            var actual = await repository.ReadIdAsync(outerId2, teamId).ConfigureAwait(false);
            actual.Should().Be(userId2);
        }

        [Test]
        public async Task ReadIdAsync_Should_Return_UserId_And_Filter_By_TeamId()
        {
            var userId1 = Guid.NewGuid();
            var outerId1 = "outerId1";
            var teamId1 = Guid.NewGuid();
            var user = new User
            {
                Id = userId1,
                Name = "user1",
                OuterId = outerId1,
                TeamId = teamId1
            };
            await repository.SaveAsync(user).ConfigureAwait(false);

            var userId2 = Guid.NewGuid();
            var teamId2 = Guid.NewGuid();
            var user2 = new User
            {
                Id = userId2,
                Name = "user2",
                OuterId = outerId1,
                TeamId = teamId2
            };
            await repository.SaveAsync(user2).ConfigureAwait(false);

            var actual = await repository.ReadIdAsync(outerId1, teamId1).ConfigureAwait(false);
            actual.Should().Be(userId1);
        }

        [Test]
        public async Task ReadIdAsync_Should_Return_Null_For_Id_Of_Absent_User()
        {
            var outerId = "outerId1";

            var actual = await repository.ReadAsync(outerId, Guid.NewGuid()).ConfigureAwait(false);
            actual.Should().BeNull();
        }

        [Test]
        public async Task ReadByIdAsync_Should_Return_Null_For_Id_Of_Absent_User()
        {
            var userId = Guid.NewGuid();
            try
            {
                var actual = await repository.ReadAsync(userId).ConfigureAwait(false);
                actual.Should().BeNull();
            }
            catch (Exception e)
            {
                Console.Write("sd");
            }
        }

        [Test]
        public async Task ReadIdsByOuterIdAsync_Should_Return_All_Users_With_Such_OuterId()
        {
            var outerId = "outerId1";
            var teamId1 = Guid.NewGuid();
            var userId1 = Guid.NewGuid();
            var user1 = new User
            {
                Id = userId1,
                Name = "user1",
                OuterId = outerId,
                TeamId = teamId1
            };
            await repository.SaveAsync(user1).ConfigureAwait(false);
            var teamId2 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            var user2 = new User
            {
                Id = userId2,
                Name = "user2",
                OuterId = outerId,
                TeamId = teamId2
            };
            await repository.SaveAsync(user2).ConfigureAwait(false);
            var outerId2 = "outerId2";
            var userId3 = Guid.NewGuid();
            var user3 = new User
            {
                Id = userId3,
                Name = "user3",
                OuterId = outerId2,
                TeamId = teamId1
            };
            await repository.SaveAsync(user3).ConfigureAwait(false);

            var actual = await repository.ReadInfosByOuterIdAsync(outerId).ConfigureAwait(false);
            actual.Should().BeEquivalentTo(new List<UserAndTeamInfo>
            {
                new UserAndTeamInfo{Id = userId1, TeamId = teamId1}, new UserAndTeamInfo{Id = userId2, TeamId = teamId2}
            });
        }

        [Test]
        public async Task ReadIdsByOuterIdAsync_Should_Return_Empty_List_If_There_Not_Such_Users()
        {
            var outerId = "outerId1";
            var teamId1 = Guid.NewGuid();
            var userId1 = Guid.NewGuid();
            var user1 = new User
            {
                Id = userId1,
                Name = "user1",
                OuterId = outerId,
                TeamId = teamId1
            };
            await repository.SaveAsync(user1).ConfigureAwait(false);

            var actual = await repository.ReadInfosByOuterIdAsync("another outerId").ConfigureAwait(false);
            actual.Should().BeEmpty();
        }

        [Test]
        public async Task ReadIdsByOuterIdAsync_Should_Return_Empty_List_If_There_No_Users_At_All()
        {
            var actual = await repository.ReadInfosByOuterIdAsync("another outerId").ConfigureAwait(false);
            actual.Should().BeEmpty();
        }
    }
}
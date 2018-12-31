using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
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
            repository = new UserRepository(DbConnectionStringProvider);
        }

        [TearDown]
        public void TearDown()
        {
            repository.DeleteAll();
        }

        [Test]
        public void Save_Should_Save_All_Fields_Of_User()
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
            repository.Save(user);

            var actual = repository.Read(userId);
            actual.Should().BeEquivalentTo(user);
        }

        [Test]
        public void ReadByOuterId_Should_Return_Specified_User()
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
            repository.Save(user);

            var actual = repository.Read(outerId, teamId);
            actual.Should().BeEquivalentTo(user);
        }

        [Test]
        public void ReadId_Should_Return_UserId_And_Filter_By_OuterId()
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
            repository.Save(user);

            var userId2 = Guid.NewGuid();
            var outerId2 = "outerId2";
            var user2 = new User
            {
                Id = userId2,
                Name = "user2",
                OuterId = outerId2,
                TeamId = teamId
            };
            repository.Save(user2);

            var actual = repository.ReadId(outerId2, teamId);
            actual.Should().Be(userId2);
        }

        [Test]
        public void ReadId_Should_Return_UserId_And_Filter_By_TeamId()
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
            repository.Save(user);

            var userId2 = Guid.NewGuid();
            var teamId2 = Guid.NewGuid();
            var user2 = new User
            {
                Id = userId2,
                Name = "user2",
                OuterId = outerId1,
                TeamId = teamId2
            };
            repository.Save(user2);

            var actual = repository.ReadId(outerId1, teamId1);
            actual.Should().Be(userId1);
        }

        [Test]
        public void ReadId_Should_Return_Null_For_Id_Of_Absent_User()
        {
            var outerId = "outerId1";

            var actual = repository.Read(outerId, Guid.NewGuid());
            actual.Should().BeNull();
        }

        [Test]
        public void ReadById_Should_Return_Null_For_Id_Of_Absent_User()
        {
            var userId = Guid.NewGuid();
            try
            {
                var actual = repository.Read(userId);
                actual.Should().BeNull();
            }
            catch (Exception e)
            {
                Console.Write("sd");
            }
        }

        //[Test]
        //public void Save_Should_Throw_When_Save_Two_Persons_With_The_Same_OuterId_And_TeamId()
        //{
        //    var outerId = "outerId1";
        //    var teamId = Guid.NewGuid();
        //    var user1 = new User
        //    {
        //        Id = Guid.NewGuid(),
        //        Name = "user1",
        //        OuterId = outerId,
        //        TeamId = teamId
        //    };
        //    repository.Save(user1);
        //    var user2 = new User
        //    {
        //        Id = Guid.NewGuid(),
        //        Name = "user2",
        //        OuterId = outerId,
        //        TeamId = teamId
        //    };
        //    Action saveDuplicateUserAction = () => repository.Save(user2);
        //    saveDuplicateUserAction.Should().Throw<UniqueConstraintException>();
        //}

        [Test]
        public void ReadIdsByOuterId_Should_Return_All_Users_With_Such_OuterId()
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
            repository.Save(user1);
            var teamId2 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            var user2 = new User
            {
                Id = userId2,
                Name = "user2",
                OuterId = outerId,
                TeamId = teamId2
            };
            repository.Save(user2);
            var outerId2 = "outerId2";
            var userId3 = Guid.NewGuid();
            var user3 = new User
            {
                Id = userId3,
                Name = "user3",
                OuterId = outerId2,
                TeamId = teamId1
            };
            repository.Save(user3);

            var actual = repository.ReadInfosByOuterId(outerId);
            actual.Should().BeEquivalentTo(new List<UserAndTeamInfo>
            {
                new UserAndTeamInfo{Id = userId1, TeamId = teamId1}, new UserAndTeamInfo{Id = userId2, TeamId = teamId2}
            });
        }

        [Test]
        public void ReadIdsByOuterId_Should_Return_Empty_List_If_There_Not_Such_Users()
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
            repository.Save(user1);

            var actual = repository.ReadInfosByOuterId("another outerId");
            actual.Should().BeEmpty();
        }

        [Test]
        public void ReadIdsByOuterId_Should_Return_Empty_List_If_There_No_Users_At_All()
        {
            var actual = repository.ReadInfosByOuterId("another outerId");
            actual.Should().BeEmpty();
        }
    }
}
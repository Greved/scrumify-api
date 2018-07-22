using System;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Scrumify.DataAccess.TeamSupport;
using Scrumify.DataAccess.TestCore;
using Scrumify.Models;

namespace Scrumify.DataAccess.Tests
{
    [TestFixture]
	public class TeamRepositoryTests: DbTestBase
	{
		private TeamRepository repository;

		[SetUp]
		public override void Setup()
		{
			base.Setup();
			repository = new TeamRepository(DbConnectionStringProvider);
		}

	    [TearDown]
	    public async Task TearDown()
	    {
	        await repository.DeleteAllAsync();
	    }

        [Test]
		public async Task ExistsAsync_Should_Return_True_For_Existent_Team()
		{
			var teamId1 = Guid.NewGuid();
			var team = new Team
			{
				Id = teamId1,
				Name = "team1"
			};
			await repository.SaveAsync(team);

			var teamId2 = Guid.NewGuid();
			var team2 = new Team
			{
				Id = teamId2,
				Name = "team2"
			};
			await repository.SaveAsync(team2);

			var exists = await repository.ExistsAsync(teamId1);
			exists.Should().BeTrue();
		}

	    [Test]
	    public async Task ExistsAsync_Should_Return_False_When_No_Teams()
	    {
	        var exists = await repository.ExistsAsync(Guid.NewGuid());
	        exists.Should().BeFalse();
	    }

	    [Test]
	    public async Task ExistsAsync_Should_Return_False_When_No_Team_With_Specified_Id()
	    {
	        var teamId1 = Guid.NewGuid();
	        var team = new Team
	        {
	            Id = teamId1,
	            Name = "team1"
	        };
	        await repository.SaveAsync(team);

	        var exists = await repository.ExistsAsync(Guid.NewGuid());
	        exists.Should().BeFalse();
	    }
    }
}
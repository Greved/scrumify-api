using System;
using System.Threading.Tasks;
using Scrumify.Models;

namespace Scrumify.DataAccess.TeamSupport
{
	public interface ITeamRepository
	{
		Task<bool> ExistsAsync(Guid teamId);
		Task SaveAsync(Team team);
		Task DeleteAllAsync();
	}
}
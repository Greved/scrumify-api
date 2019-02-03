using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Scrumify.Models;

namespace Scrumify.DataAccess.UserSupport
{
    public interface IUserRepository
    {
        Task SaveAsync(User user);
        Task<User> ReadAsync(Guid id);
        Task<Guid> ReadIdAsync(string outerId, Guid teamId);
        Task<User> ReadAsync(string outerId, Guid teamId);
        Task<List<UserAndTeamInfo>> ReadInfosByOuterIdAsync(string outerId);
    }
}
using System;
using System.Collections.Generic;
using Scrumify.Models;

namespace Scrumify.DataAccess.UserSupport
{
    public interface IUserRepository
    {
        void Save(User user);
        User Read(Guid id);
        Guid ReadId(string outerId, Guid teamId);
        User Read(string outerId, Guid teamId);
        List<UserAndTeamInfo> ReadInfosByOuterId(string outerId);
    }
}
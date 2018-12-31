using System;

namespace Scrumify.Models
{
    [Serializable]
    public class UserAndTeamInfo
    {
        public Guid Id { get; set; }
        public Guid TeamId { get; set; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(TeamId)}: {TeamId}";
        }
    }
}
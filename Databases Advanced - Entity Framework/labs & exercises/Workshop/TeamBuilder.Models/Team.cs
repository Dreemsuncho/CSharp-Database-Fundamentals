using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TeamBuilder.Models
{
    public class Team
    {
        public Team()
        {
            UserTeams = new HashSet<UserTeam>();
            Events = new HashSet<EventTeam>();
            Invitations = new HashSet<Invitation>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [MinLength(3)]
        public string Acronym { get; set; }

        public int CreatorId { get; set; }
        public User Creator { get; set; }

        public ICollection<UserTeam> UserTeams { get; set; }
        public ICollection<EventTeam> Events { get; set; }
        public ICollection<Invitation> Invitations { get; set; }
    }
}

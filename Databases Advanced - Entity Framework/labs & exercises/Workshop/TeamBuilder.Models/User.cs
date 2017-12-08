using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace TeamBuilder.Models
{
    public class User
    {
        public User()
        {
            CreatedEvents = new HashSet<Event>();
            CreatedTeams = new HashSet<Team>();
            UserTeams = new HashSet<UserTeam>();
            ReceivedInvitaions = new HashSet<Invitation>();
        }

        public int Id { get; set; }
        [MinLength(3)]
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [MinLength(6), RegularExpression("(?=.*[A-Z])(?=.*[0-9]).*")]
        public string Password { get; set; }
        public Gender Gender { get; set; }
        public int Age { get; set; }
        public bool IsDeleted { get; set; }

        public ICollection<Event> CreatedEvents { get; set; }
        public ICollection<Team> CreatedTeams { get; set; }
        public ICollection<UserTeam> UserTeams { get; set; }
        public ICollection<Invitation> ReceivedInvitaions { get; set; }
    }
}

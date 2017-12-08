using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TeamBuilder.Models
{
    public class Event
    {
        private DateTime _endDate;

        public Event()
        {
            ParticipatingEventTeams = new HashSet<EventTeam>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime StartDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime EndDate
        {
            get => _endDate; set
            {
                if (value <= StartDate)
                    throw new InvalidOperationException("End date must be after start date!");
                _endDate = value;
            }
        }

        public int CreatorId { get; set; }
        public User Creator { get; set; }

        public ICollection<EventTeam> ParticipatingEventTeams { get; set; }
    }
}

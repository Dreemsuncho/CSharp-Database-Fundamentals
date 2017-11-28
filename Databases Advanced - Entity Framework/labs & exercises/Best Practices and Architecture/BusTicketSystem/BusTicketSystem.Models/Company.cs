using System.Collections.Generic;
using BusTicketSystem.Models.Abstracts;

namespace BusTicketSystem.Models
{
    public class Company : EntityBase
    {
        public Company()
        {
            Trips = new HashSet<Trip>();
            Reviews = new HashSet<Review>();
        }

        public string Name { get; set; }
        public string Nationality { get; set; }
        public int Rating { get; set; }

        public IEnumerable<Trip> Trips { get; set; }
        public IEnumerable<Review> Reviews { get; set; }
    }
}

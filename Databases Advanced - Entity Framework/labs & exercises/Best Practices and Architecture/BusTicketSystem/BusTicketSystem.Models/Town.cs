using System.Collections.Generic;
using BusTicketSystem.Models.Abstracts;

namespace BusTicketSystem.Models
{
    public class Town : EntityBase
    {
        public Town()
        {
            CustomerHomeTowns = new HashSet<Customer>();
            BusStations = new HashSet<BusStation>();
        }

        public string Name { get; set; }
        public string Country { get; set; }

        public IEnumerable<Customer> CustomerHomeTowns { get; set; }
        public IEnumerable<BusStation> BusStations { get; set; }
    }
}
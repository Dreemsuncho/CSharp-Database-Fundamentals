using System.Collections.Generic;
using BusTicketSystem.Models.Abstracts;

namespace BusTicketSystem.Models
{
    public class BusStation : EntityBase
    {
        public BusStation()
        {
            OriginTrips = new HashSet<Trip>();
            DestinationTrips = new HashSet<Trip>();
            OriginArrivedTrips = new HashSet<ArrivedTrip>();
            DestinationArrivedTrips = new HashSet<ArrivedTrip>();
        }

        public string Name { get; set; }

        public Town Town { get; set; }
        public int TownId { get; set; }

        public IEnumerable<Trip> OriginTrips { get; set; }
        public IEnumerable<Trip> DestinationTrips { get; set; }

        public IEnumerable<ArrivedTrip> OriginArrivedTrips { get; set; }
        public IEnumerable<ArrivedTrip> DestinationArrivedTrips { get; set; }
    }
}

using System;
using BusTicketSystem.Models.Abstracts;

namespace BusTicketSystem.Models
{
    public class ArrivedTrip : EntityBase
    {
        public DateTime ArrivalTime { get; set; }
        public int PassengersCount { get; set; }

        public BusStation OriginBusStation { get; set; }
        public int OriginBusStationId { get; set; }

        public BusStation DestinationBusStation { get; set; }
        public int DestinationBusStationId { get; set; }
    }
}
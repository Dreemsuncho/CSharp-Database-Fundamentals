using System;
using System.Collections.Generic;
using BusTicketSystem.Models.Enums;
using BusTicketSystem.Models.Abstracts;

namespace BusTicketSystem.Models
{
    public class Trip : EntityBase
    {
        public Trip()
        {
            Tickets = new HashSet<Ticket>();
        }

        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public Status Status { get; set; }

        public BusStation OriginBusStation { get; set; }
        public int OriginBusStationId { get; set; }

        public BusStation DestinationBusStation { get; set; }
        public int DestinationBusStationId { get; set; }

        public Company BusCompany { get; set; }
        public int BusCompanyId { get; set; }

        public IEnumerable<Ticket> Tickets { get; set; }
    }
}
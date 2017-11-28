using BusTicketSystem.Models.Abstracts;

namespace BusTicketSystem.Models
{
    public class Ticket : EntityBase
    {
        public decimal Price { get; set; }
        public int Seat { get; set; }

        public Customer Customer { get; set; }
        public int CustomerId { get; set; }

        public Trip Trip { get; set; }
        public int TripId { get; set; }
    }
}

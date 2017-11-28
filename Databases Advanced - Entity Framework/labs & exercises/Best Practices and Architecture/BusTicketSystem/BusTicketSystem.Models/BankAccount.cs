using BusTicketSystem.Models.Abstracts;

namespace BusTicketSystem.Models
{
    public class BankAccount : EntityBase
    {
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }

        public Customer Customer { get; set; }
        public int CustomerId { get; set; }
    }
}

using System;
using BusTicketSystem.Models.Enums;
using BusTicketSystem.Models.Abstracts;
using System.Collections.Generic;

namespace BusTicketSystem.Models
{
    public class Customer : EntityBase
    {
        public Customer()
        {
            Tickets = new HashSet<Ticket>();
            Reviews = new HashSet<Review>();
        }

        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }

        public Town HomeTown { get; set; }
        public int HomeTownId { get; set; }

        public BankAccount BankAccount { get; set; }
        public int? BankAccountId { get; set; }

        public IEnumerable<Ticket> Tickets { get; set; }
        public IEnumerable<Review> Reviews { get; set; }
    }
}

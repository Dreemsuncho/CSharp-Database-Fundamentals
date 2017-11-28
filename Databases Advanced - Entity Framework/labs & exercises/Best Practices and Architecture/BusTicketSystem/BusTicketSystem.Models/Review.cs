using System;
using BusTicketSystem.Models.Abstracts;

namespace BusTicketSystem.Models
{
    public class Review : EntityBase
    {
        public string Content { get; set; }
        public double Grade { get; set; }
        public DateTime PublishDate { get; set; }
//s        
        public Company Company { get; set; }
        public int CompanyId { get; set; }

        public Customer Customer { get; set; }
        public int CustomerId { get; set; }
    }
}

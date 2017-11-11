using System.Collections.Generic;

namespace P04_05_SalesDatabaseMigrations.Data.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public double Quantity { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public ICollection<Sale> Sales { get; set; }
    }
}

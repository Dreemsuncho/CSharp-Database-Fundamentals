using System.Collections.Generic;

namespace P04_05_SalesDatabaseMigrations.Data.Models
{
    public class Store
    {
        public int StoreId { get; set; }
        public string Name { get; set; }
        public ICollection<Sale> Sales { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ProductsShop.Models
{
    public class Product
    {
        public Product()
        {
            CategoryProducts = new HashSet<CategoryProduct>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        
        public User Buyer { get; set; }
        public int? BuyerId { get; set; }

        public User Seller { get; set; }
        public int SellerId { get; set; }

        public ICollection<CategoryProduct> CategoryProducts { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ProductsShop.DTOs
{
    class ProductInRangeWithSellerDTO
    {
        public string Name { get; internal set; }
        public decimal Price { get; internal set; }
        public string Seller { get; internal set; }
    }
}

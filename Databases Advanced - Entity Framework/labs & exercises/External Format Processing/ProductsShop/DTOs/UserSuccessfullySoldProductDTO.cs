using System;
using System.Collections.Generic;
using System.Text;

namespace ProductsShop.DTOs
{
    class UserSuccessfullySoldProductDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public IEnumerable<SoldProductDTO> SoldProducts { get; set; }
    }

    class SoldProductDTO
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string BuyerFirstName { get; set; }
        public string BuyerLastName { get; set; }
    }
}

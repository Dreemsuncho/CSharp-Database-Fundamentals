using System.Collections.Generic;

namespace ProductsShop.DTOs
{
    class UserProductDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public ICollection<UserSoldProductDTO> SoldProducts { get; set; }
    }

    class UserSoldProductDTO
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}

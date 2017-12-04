namespace ProductsShop.DTOs
{
    class CategoryDTO
    {
        public string Category { get; set; }
        public int ProductsCount { get; set; }
        public decimal AveragePrice { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}

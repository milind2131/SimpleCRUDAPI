namespace SimpleCRUDAPI.DTO_s
{
    public class ProductRequestDto
    {
        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        public string? Description { get; set; }

        public int StockQuantity { get; set; }
    }
}

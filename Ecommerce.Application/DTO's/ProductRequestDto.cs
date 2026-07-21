namespace SimpleCRUDAPI.DTO_s
{
    public class ProductRequestDto
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Category { get; set; } = string.Empty;
    }
}

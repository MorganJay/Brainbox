namespace Contracts.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public int NumberInStock { get; set; }

        public string Category { get; set; }
    }

    public class CreateProductDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int NumberInStock { get; set; }
        public string Category { get; set; }
        public bool Stockable { get; set; }

    }

    public class GetProductQuery
    {
        public string? Name { get; set; }
        public string? Category { get; set; }

        public int Page { get; set; } = 1;
        public int Size { get; set; } = 20;
    }
}

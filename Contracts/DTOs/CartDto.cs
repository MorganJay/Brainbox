namespace Contracts.DTOs
{
    public class CartDto
    {
        public Guid UserId { get; set; }

        public ICollection<CartItemDto> Products { get; set; }

        public decimal TotalAmount => Products.Sum(x => x.Quantity * x.ProductPrice);
    }

    public class CartItemDto
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }

        public int Quantity { get; set; }
    }

    public class CreateCartDto
    {
        public Guid UserId { get; set; }

        public ICollection<CreateCartItemDto> Products { get; set; }
    }

    public class CreateCartItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}

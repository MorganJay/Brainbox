using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("CartItems")]
    public class CartItem : BaseEntity<int>
    {
        public int CartId { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }

        public int Quantity { get; set; }

        public Cart Cart { get; set; }

        public Product Product { get; set; }
    }
}
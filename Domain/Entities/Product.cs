using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Products")]
    public class Product : DatedEntity<int, DateTime, DateTime>
    {
        public string Name { get; set; }
        public decimal Price { get; set; }

        public int NumberInStock { get; set; }

        public string Category { get; set; }

        public bool Stockable { get; set; }
    }
}
namespace Domain.Entities
{
    public class Cart : DatedEntity<int, DateTime, DateTime>
    {
        public Guid UserId { get; set; }

        public virtual ICollection<CartItem> Products { get; set; }

        public virtual User User { get; set; }
    }
}
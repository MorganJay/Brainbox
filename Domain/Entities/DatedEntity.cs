namespace Domain.Entities
{
    public abstract class DatedEntity<T, TCreated> : BaseEntity<T>
    {
        public TCreated CreatedOn { get; set; } = default!;
    }

    public abstract class DatedEntity<T, TCreated, TModified> : DatedEntity<T, TCreated>
    {
        public TModified LastModified { get; set; } = default!;
    }
}
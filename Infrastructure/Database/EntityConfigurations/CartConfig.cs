using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.EntityConfigurations
{
    public class CartConfig : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            //cart items
            builder.HasMany(x => x.Products).WithOne(x => x.Cart);

            builder.Navigation(x => x.Products).AutoInclude();

            builder.HasIndex(x => new { x.UserId }).IsUnique();

            // user
            builder.HasOne(x => x.User).WithOne();
            builder.Navigation(x => x.User).AutoInclude();
        }
    }
}

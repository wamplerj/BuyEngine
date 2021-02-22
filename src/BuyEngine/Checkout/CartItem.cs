using BuyEngine.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BuyEngine.Checkout
{
    public class CartItem
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }

        public decimal Total => Quantity * Product.Price;
    }

    public class CartItemTypeConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.ToTable("CartItems");
            builder.HasKey(ci => ci.Id);
            builder.Property(ci => ci.Quantity).IsRequired();
            builder.HasOne<Product>().WithMany();
        }
    }
}
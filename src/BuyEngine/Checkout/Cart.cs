using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace BuyEngine.Checkout
{
    public class Cart
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Expires { get; set; }

        public List<CartItem> Items { get; set; }
    }

    public class CartTypeConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property<int>("ClusterId").ValueGeneratedOnAdd();
            builder.Property<int>("ClusterId").IsRequired();
            builder.HasIndex("ClusterId").IsUnique();
            builder.Property(c => c.Created).IsRequired().HasDefaultValueSql("getutcdate()");
            builder.Property(c => c.Expires);
            builder.HasMany<CartItem>(c => c.Items).WithOne().HasForeignKey("CartID");
        }
    }
}
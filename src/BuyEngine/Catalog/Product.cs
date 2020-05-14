using BuyEngine.Catalog.Suppliers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BuyEngine.Catalog
{
    public class Product
    {

        public int Id { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public bool Enabled { get; set; }

        public Brand Brand { get; set; }
        public Supplier Supplier { get; set; }
        
    }

    public class ProductTypeConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Sku).HasMaxLength(100).IsRequired();
            builder.HasIndex(b => b.Sku).IsUnique();
            builder.Property(b => b.Name).HasMaxLength(200).IsRequired();
            builder.Property(b => b.Description).HasMaxLength(500);
            builder.Property(b => b.Price).IsRequired();
            builder.Property(b => b.Quantity).IsRequired();
            builder.Property(b => b.Enabled).IsRequired().HasDefaultValue(true);
            builder.HasOne(b => b.Supplier).WithMany(p => p.Products);
            builder.HasOne(b => b.Brand).WithMany(p => p.Products);
        }
    }

}

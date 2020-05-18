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

        public bool Enabled { get; set; } = true;

        public int BrandId { get; set; }
        public Brand Brand { get; set; }

        public int SupplierId { get; set; }
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
            builder.Property(b => b.Price).IsRequired().HasColumnType("decimal(18,4)");
            builder.Property(b => b.Quantity).IsRequired();
            builder.Property(b => b.Enabled).IsRequired();
            builder.HasOne(s => s.Supplier).WithMany(p => p.Products).HasForeignKey("SupplierId");
            builder.HasOne(b => b.Brand).WithMany(p => p.Products).HasForeignKey("BrandId");
            builder.HasData(new Product {Id = 1, Sku = "APP-IPH-001", Name = "iPhone 11", Price = 699m, Quantity = 75, BrandId = 1, SupplierId = 1},
                new Product {Id = 2, Sku = "APP-IPH-002", Name = "iPhone 11 Pro", Quantity = 125, Price = 999m, BrandId = 1, SupplierId = 1 },
                new Product {Id = 3, Sku = "SAM-S20-001", Name = "Samsung S20", Price = 999m, Quantity = 15, BrandId = 2, SupplierId = 2 },
                new Product {Id = 4, Sku = "SAM-S20-002", Name = "Samsung S20+", Price = 1199m, Quantity = 25, BrandId = 2, SupplierId = 2 },
                new Product {Id = 5, Sku = "SAM-S20-003", Name = "Samsung S20 Ultra", Price = 1399m, Quantity = 20, BrandId = 2, SupplierId = 2 },
                new Product {Id = 6, Sku = "GGL-PXL-001", Name = "Google Pixel 4", Price = 799m, Quantity = 25, BrandId = 3, SupplierId = 3 },
                new Product {Id = 7, Sku = "GGL-PXL-002", Name = "Google Pixel 4 XL", Price = 899m, Quantity = 20, BrandId = 3, SupplierId = 4 }
            );
        }
    }
}
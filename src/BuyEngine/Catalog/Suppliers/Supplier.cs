using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace BuyEngine.Catalog.Suppliers
{
    public class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string WebsiteUrl { get; set; }
        public string Notes { get; set; }
        public List<Product> Products { get; set; }
    }

    public class SupplierTypeConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).ValueGeneratedOnAdd();
            builder.Property(s => s.Name).HasMaxLength(200).IsRequired();
            builder.Property(s => s.WebsiteUrl).HasMaxLength(200);
            builder.Property(s => s.Notes).HasMaxLength(2000);
            builder.HasData(
                new Supplier { Id = 1, Name = "Apple",  WebsiteUrl = "https://www.apple.com"},
                new Supplier { Id = 2, Name = "Samsung",  WebsiteUrl = "https://www.samsung.com/us" },
                new Supplier { Id = 3, Name = "Ebay", WebsiteUrl = "https://www.ebay.com" },
                new Supplier { Id = 4, Name = "Alibaba",  WebsiteUrl = "https://www.alibaba.com" }
                );
        }
    }
}

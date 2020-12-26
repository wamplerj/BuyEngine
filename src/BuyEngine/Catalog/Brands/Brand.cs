using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BuyEngine.Catalog.Brands
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string WebsiteUrl { get; set; }
        public string Notes { get; set; }
        public List<Product> Products { get; set; }
    }

    public class BrandTypeConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.ToTable("Brands");
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Name).HasMaxLength(200).IsRequired();
            builder.Property(b => b.WebsiteUrl).HasMaxLength(200);
            builder.Property(b => b.Notes).HasMaxLength(2000);
            builder.HasData(new Brand()
                {
                    Id = 1,
                    Name = "Apple",
                    WebsiteUrl = "https://www.apple.com",
                    Notes = "Makes overpriced phones that sell very well."
                },
                new Brand()
                {
                    Id = 2,
                    Name = "Samsung",
                    WebsiteUrl = "https://www.samsung.com/us"
                },
                new Brand()
                {
                    Id = 3,
                    Name = "Google",
                    WebsiteUrl = "https://store.google.com/us/category/phones"
                }
            );
        }
    }
}

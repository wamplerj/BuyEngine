﻿// <auto-generated />
using BuyEngine.Catalog;
using BuyEngine.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BuyEngine.Migrations
{
    [DbContext(typeof(StoreDbContext))]
    [Migration("20200514224725_SeedingTestProducts")]
    partial class SeedingTestProducts
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("BuyEngine")
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BuyEngine.Catalog.Brand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(2000)")
                        .HasMaxLength(2000);

                    b.Property<string>("WebsiteUrl")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.ToTable("Brands");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Apple",
                            Notes = "Makes overpriced phones that sell very well.",
                            WebsiteUrl = "https://www.apple.com"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Samsung",
                            WebsiteUrl = "https://www.samsung.com/us"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Google",
                            WebsiteUrl = "https://store.google.com/us/category/phones"
                        });
                });

            modelBuilder.Entity("BuyEngine.Catalog.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BrandId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<bool>("Enabled")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,4)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("Sku")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<int>("SupplierId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BrandId");

                    b.HasIndex("Sku")
                        .IsUnique();

                    b.HasIndex("SupplierId");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            BrandId = 1,
                            Enabled = true,
                            Name = "iPhone 11",
                            Price = 699m,
                            Quantity = 75,
                            Sku = "APP-IPH-001",
                            SupplierId = 1
                        },
                        new
                        {
                            Id = 2,
                            BrandId = 1,
                            Enabled = true,
                            Name = "iPhone 11 Pro",
                            Price = 999m,
                            Quantity = 125,
                            Sku = "APP-IPH-002",
                            SupplierId = 1
                        },
                        new
                        {
                            Id = 3,
                            BrandId = 2,
                            Enabled = true,
                            Name = "Samsung S20",
                            Price = 999m,
                            Quantity = 15,
                            Sku = "SAM-S20-001",
                            SupplierId = 2
                        },
                        new
                        {
                            Id = 4,
                            BrandId = 2,
                            Enabled = true,
                            Name = "Samsung S20+",
                            Price = 1199m,
                            Quantity = 25,
                            Sku = "SAM-S20-002",
                            SupplierId = 2
                        },
                        new
                        {
                            Id = 5,
                            BrandId = 2,
                            Enabled = true,
                            Name = "Samsung S20 Ultra",
                            Price = 1399m,
                            Quantity = 20,
                            Sku = "SAM-S20-003",
                            SupplierId = 2
                        },
                        new
                        {
                            Id = 6,
                            BrandId = 3,
                            Enabled = true,
                            Name = "Google Pixel 4",
                            Price = 799m,
                            Quantity = 25,
                            Sku = "GGL-PXL-001",
                            SupplierId = 3
                        },
                        new
                        {
                            Id = 7,
                            BrandId = 3,
                            Enabled = true,
                            Name = "Google Pixel 4 XL",
                            Price = 899m,
                            Quantity = 20,
                            Sku = "GGL-PXL-002",
                            SupplierId = 4
                        });
                });

            modelBuilder.Entity("BuyEngine.Catalog.Suppliers.Supplier", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(2000)")
                        .HasMaxLength(2000);

                    b.Property<string>("WebsiteUrl")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.ToTable("Suppliers");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Apple",
                            WebsiteUrl = "https://www.apple.com"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Samsung",
                            WebsiteUrl = "https://www.samsung.com/us"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Ebay",
                            WebsiteUrl = "https://www.ebay.com"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Alibaba",
                            WebsiteUrl = "https://www.alibaba.com"
                        });
                });

            modelBuilder.Entity("BuyEngine.Catalog.Product", b =>
                {
                    b.HasOne("BuyEngine.Catalog.Brand", "Brand")
                        .WithMany("Products")
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BuyEngine.Catalog.Suppliers.Supplier", "Supplier")
                        .WithMany("Products")
                        .HasForeignKey("SupplierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}

using BuyEngine.Catalog;
using BuyEngine.Catalog.Suppliers;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Linq;

namespace BuyEngine.Tests.Unit.Catalog
{
    public class ProductServiceTests
    {
        private CatalogDbContext _catalogDbContext;
        private ProductService _productService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CatalogDbContext>()
                                .UseInMemoryDatabase($"{nameof(ProductServiceTests)}-{Guid.NewGuid()}").Options;
            _catalogDbContext = new CatalogDbContext(options);

            _productService = new ProductService(_catalogDbContext, new ProductValidator(_catalogDbContext));

            _catalogDbContext.Brands.Add(new Brand() {Id = 1, Name = "Test Brand 1"});
            _catalogDbContext.Suppliers.Add(new Supplier() { Id = 1, Name = "Test Brand 1" });
        }

        [Test]
        public void A_Valid_Product_Can_Be_Created()
        {
            var product = new Product
            {
                Sku = "ABC-123",
                Name = "Simple Test Product"
            };

            var result = _productService.Add(product);

            Assert.That(result, Is.GreaterThan(0));
            Assert.That(_catalogDbContext.Products.FirstOrDefault(p => p.Sku.Equals(product.Sku, StringComparison.OrdinalIgnoreCase)), Is.Not.Null);
        }

        [Test]
        public void A_Product_Can_Be_Found_By_Id()
        {
            _catalogDbContext.Add(new Product() { Id = 1, Sku = "ABC-123", BrandId = 1, SupplierId = 1});
            _catalogDbContext.SaveChanges();

            var result = _productService.GetAsync(1).Result;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
        }

        [Test]
        public void A_Product_Can_Be_Found_By_Sku()
        {
            _catalogDbContext.Add(new Product() {Id = 1, Sku = "ABC-123", BrandId = 1, SupplierId = 1 });
            _catalogDbContext.SaveChanges();

            var result = _productService.GetAsync("ABC-123").Result;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Sku, Is.EqualTo("ABC-123"));
        }
    }
}
using BuyEngine.Catalog;
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
            var options = new DbContextOptionsBuilder<CatalogDbContext>().UseInMemoryDatabase(nameof(ProductServiceTests)).Options;
            _catalogDbContext = new CatalogDbContext(options);

            _productService = new ProductService(_catalogDbContext, new ProductValidator(_catalogDbContext));
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
    }
}
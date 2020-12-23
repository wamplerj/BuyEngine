using BuyEngine.Catalog;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Linq;

namespace BuyEngine.Tests.Unit.Catalog
{
    public class ProductValidatorTests
    {
        private CatalogDbContext _catalogDbContext;
        private ProductValidator _validator;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CatalogDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            _catalogDbContext = new CatalogDbContext(options);

            _validator = new ProductValidator(_catalogDbContext);
        }

        [Test]
        public void A_Valid_Product_Returns_IsValid_And_Has_No_Messages()
        {
            var product = new Product
            {
                Sku = "ABC-123",
                Name = "Test Product"
            };


            var result = _validator.Validate(product);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Messages, Is.Empty);
        }

        [Test]
        public void A_Product_With_No_Sku_Fails_Validation()
        {
            var product = new Product
            {
                Name = "Test Product"
            };

            var result = _validator.Validate(product);
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Messages.Count, Is.EqualTo(1));
            Assert.That(result.Messages.First().Key, Is.EqualTo(nameof(product.Sku)));
        }

        [Test]
        public void A_Product_Sku_Can_Be_Verified_Unique()
        {
            var sku = "ABC-123";

            _catalogDbContext.Products.Add(new Product {Sku = sku});
            _catalogDbContext.SaveChanges();

            var result = _validator.IsSkuUnique(sku);
            Assert.That(result, Is.False);
        }
    }
}
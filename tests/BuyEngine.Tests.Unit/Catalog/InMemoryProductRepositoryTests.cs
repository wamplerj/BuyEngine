using BuyEngine.Catalog;
using BuyEngine.Catalog.Brands;
using BuyEngine.Catalog.Suppliers;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace BuyEngine.Tests.Unit.Catalog
{
    public class InMemoryProductRepositoryTests
    {
        private InMemoryProductRepository _repository;

        [SetUp]
        public async Task SetUp()
        {
            _repository = new InMemoryProductRepository(null);
        }

        [Test]
        public async Task A_Product_Can_Be_Added_To_The_Repository()
        {
            var product = new Product();

            var id = await _repository.AddAsync(product);
            Assert.That(id, Is.Not.EqualTo(Guid.Empty));
        }

        [Test]
        public async Task A_Duplicate_Product_Can_Not_Be_Added_To_The_Repository()
        {
            var duplicateSku = "TST-SKU-001";

            var product1 = new Product { Sku = duplicateSku };
            var product2 = new Product { Sku = duplicateSku };

            var id = await _repository.AddAsync(product1);
            Assert.That(id, Is.Not.EqualTo(Guid.Empty));

            Assert.ThrowsAsync<ArgumentException>(() => _repository.AddAsync(product2));
        }

        [Test]
        public async Task A_Product_Can_Be_Updated_In_The_Repository()
        {
            var product = new Product { Sku = "TST-SKU-001" };

            var id = await _repository.AddAsync(product);

            var updated = await _repository.UpdateAsync(new Product { Sku = "TST-SKU-00X" });
            Assert.That(updated, Is.False);

            updated = await _repository.UpdateAsync(new Product { Id = id, Sku = "TST-SKU-001", Description = "Blah" });
            Assert.That(updated, Is.True);
        }

        [Test]
        public async Task A_Product_Can_Be_Removed_From_The_Repository()
        {
            var product = new Product { Sku = "TST-SKU-001" };
            var id = await _repository.AddAsync(product);

            await _repository.RemoveAsync(product);

            var exists = await _repository.ExistsAsync(product.Sku);
            Assert.That(exists, Is.False);
        }

        [Test]
        public async Task An_Existing_Product_Can_Be_Found()
        {
            var product = new Product { Sku = "TST-SKU-001" };
            var id = await _repository.AddAsync(product);

            var exists = await _repository.ExistsAsync(product.Sku.ToLower());
            Assert.That(exists, Is.True);
        }

        [Test]
        public async Task A_Product_Can_Be_Found_By_Sku()
        {
            var product = new Product { Sku = "TST-SKU-001" };
            var id = await _repository.AddAsync(product);

            var foundProduct = await _repository.GetAsync(product.Sku.ToLower());
            Assert.That(foundProduct, Is.Not.Null);
            Assert.That(foundProduct.Sku, Is.EqualTo(product.Sku));

            var invalidProduct = await _repository.GetAsync(string.Empty);
            Assert.That(invalidProduct, Is.Null);
        }

        [Test]
        public async Task A_Product_Can_Be_Found_By_Id()
        {
            var product = new Product { Sku = "TST-SKU-001" };
            var id = await _repository.AddAsync(product);

            var foundProduct = await _repository.GetAsync(id);
            Assert.That(foundProduct, Is.Not.Null);
            Assert.That(foundProduct.Sku, Is.EqualTo(product.Sku));

            var invalidProduct = await _repository.GetAsync(Guid.Empty);
            Assert.That(invalidProduct, Is.Null);
        }

        [Test]
        public async Task All_Products_Can_Be_Returned()
        {
            for (var i = 1; i <= 20; i++)
            {
                var product = new Product
                {
                    Id = Guid.NewGuid(),
                    Sku = $"TST-SKU-{i}"
                };
                await _repository.AddAsync(product);
            }

            var products = await _repository.GetAllAsync(10, 1);

            Assert.That(products.Count, Is.EqualTo(10));
            Assert.That(products[4].Sku, Is.EqualTo("TST-SKU-5"));

            products = await _repository.GetAllAsync(10, 2);
            Assert.That(products.Count, Is.EqualTo(10));
            Assert.That(products[4].Sku, Is.EqualTo("TST-SKU-15"));
        }

        [Test]
        public async Task All_Products_By_Brand_Can_Be_Returned()
        {
            var brand1 = new Brand
            {
                Id = Guid.NewGuid(),
                Name = "Brand A"
            };

            var brand2 = new Brand
            {
                Id = Guid.NewGuid(),
                Name = "Brand B"
            };

            for (var i = 1; i <= 20; i++)
            {
                var product = new Product
                {
                    Id = Guid.NewGuid(),
                    Sku = $"TST-SKU-{i}",
                    Brand = i % 2 == 0 ? brand1 : brand2
                };

                await _repository.AddAsync(product);
            }

            var products = await _repository.GetAllByBrandAsync(brand1.Id, 10, 1);

            Assert.That(products.Count, Is.EqualTo(10));
            Assert.That(products[0].Sku, Is.EqualTo("TST-SKU-2"));
            Assert.That(products[9].Sku, Is.EqualTo("TST-SKU-20"));
        }

        [Test]
        public async Task All_Products_By_Supplier_Can_Be_Returned()
        {
            var supplier1 = new Supplier
            {
                Id = Guid.NewGuid(),
                Name = "Supplier A"
            };

            var supplier2 = new Supplier
            {
                Id = Guid.NewGuid(),
                Name = "Supplier B"
            };

            for (var i = 1; i <= 20; i++)
            {
                var product = new Product
                {
                    Id = Guid.NewGuid(),
                    Sku = $"TST-SKU-{i}",
                    Supplier = i % 2 == 0 ? supplier1 : supplier2
                };

                await _repository.AddAsync(product);
            }

            var products = await _repository.GetAllBySupplierAsync(supplier1.Id, 10, 1);

            Assert.That(products.Count, Is.EqualTo(10));
            Assert.That(products[0].Sku, Is.EqualTo("TST-SKU-2"));
            Assert.That(products[9].Sku, Is.EqualTo("TST-SKU-20"));
        }
    }
}

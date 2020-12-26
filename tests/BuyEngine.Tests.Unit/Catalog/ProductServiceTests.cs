using BuyEngine.Catalog;
using BuyEngine.Catalog.Suppliers;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuyEngine.Catalog.Brands;

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
        public async Task A_Product_Can_Be_Found_By_Id_Async()
        {
            _catalogDbContext.Add(new Product() { Id = 1, Sku = "ABC-123", BrandId = 1, SupplierId = 1});
            await _catalogDbContext.SaveChangesAsync();

            var result = await _productService.GetAsync(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
        }

        [Test]
        public void A_Product_Can_Be_Found_By_Id()
        {
            _catalogDbContext.Add(new Product() { Id = 1, Sku = "ABC-123", BrandId = 1, SupplierId = 1 });
            _catalogDbContext.SaveChanges();

            var result = _productService.Get(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
        }

        [Test]
        public async Task A_Product_Can_Be_Found_By_Sku_Async()
        {
            _catalogDbContext.Add(new Product() {Id = 1, Sku = "ABC-123", BrandId = 1, SupplierId = 1 });
            await _catalogDbContext.SaveChangesAsync();

            var result = await _productService.GetAsync("ABC-123");

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Sku, Is.EqualTo("ABC-123"));
        }

        [Test]
        public void A_Product_Can_Be_Found_By_Sku()
        {
            _catalogDbContext.Add(new Product() { Id = 1, Sku = "ABC-123", BrandId = 1, SupplierId = 1 });
            _catalogDbContext.SaveChanges();

            var result = _productService.Get("ABC-123");

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Sku, Is.EqualTo("ABC-123"));
        }

        [Test]
        public async Task Getting_All_Products_Async_Returns_First_Page_Only()
        {
            var products = GetProductsForPaging();
            foreach (var product in products)
            {
                await _catalogDbContext.Products.AddAsync(product);
            }

            await _catalogDbContext.SaveChangesAsync();

            var result = await _productService.GetAllAsync(5, 0);
            Assert.That(result.Count, Is.EqualTo(5));
            Assert.That(result.First().Id, Is.EqualTo(1));
            Assert.That(result.Last().Id, Is.EqualTo(5));
        }

        [Test]
        public void Getting_All_Products_Returns_First_Page_Only()
        {
            var products = GetProductsForPaging();
            foreach (var product in products)
            {
                _catalogDbContext.Products.Add(product);
            }

            _catalogDbContext.SaveChanges();

            var result = _productService.GetAll(5, 0);
            Assert.That(result.Count, Is.EqualTo(5));
            Assert.That(result.First().Id, Is.EqualTo(1));
            Assert.That(result.Last().Id, Is.EqualTo(5));
        }

        [Test]
        public async Task Getting_All_Products_By_SupplierId_Async_Returns_First_Page_Only()
        {
            var products = GetProductsForPaging();
            foreach (var product in products)
            {
                await _catalogDbContext.Products.AddAsync(product);
            }

            await _catalogDbContext.SaveChangesAsync();

            var result = await _productService.GetAllBySupplierAsync(1, 5, 0);
            Assert.That(result.Count, Is.EqualTo(5));
            Assert.That(result.First().Id, Is.EqualTo(1));
            Assert.That(result.Last().Id, Is.EqualTo(5));
        }

        [Test]
        public void Getting_All_Products_By_SupplierId_Returns_First_Page_Only()
        {
            var products = GetProductsForPaging();
            foreach (var product in products)
            {
                _catalogDbContext.Products.Add(product);
            }

            _catalogDbContext.SaveChanges();

            var result = _productService.GetAllBySupplier(1, 5, 0);
            Assert.That(result.Count, Is.EqualTo(5));
            Assert.That(result.First().Id, Is.EqualTo(1));
            Assert.That(result.Last().Id, Is.EqualTo(5));
        }
        
        [Test]
        public async Task Product_Sku_Can_Be_Verified_As_Unique_Async()
        {
            await _catalogDbContext.AddAsync(new Product() { Id = 1, Sku = "ABC-123", BrandId = 1, SupplierId = 1 });
            await _catalogDbContext.SaveChangesAsync();

            var result = await _productService.IsSkuUniqueAsync("ABC-123");
            Assert.That(result, Is.False);
        }

        [Test]
        public void Product_Sku_Can_Be_Verified_As_Unique()
        {
            _catalogDbContext.Add(new Product() { Id = 1, Sku = "ABC-123", BrandId = 1, SupplierId = 1 });
            _catalogDbContext.SaveChanges();

            var result = _productService.IsSkuUnique("ABC-123");
            Assert.That(result, Is.False);
        }

        private IEnumerable<Product> GetProductsForPaging()
        {
            var products = new List<Product>();
            for (var i = 1; i <= 10; i++)
            {
                var product = new Product()
                {
                    Id = i,
                    Name = $"Product {i}",
                    SupplierId = 1
                };
                products.Add(product);
            }

            return products;
        }
    }
}
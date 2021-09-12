using BuyEngine.Catalog;
using BuyEngine.Common;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuyEngine.Tests.Unit.Catalog
{
    public class ProductServiceTests
    {
        private Mock<IProductRepository> _productRepository;
        private ProductService _productService;

        [SetUp]
        public void Setup()
        {
            _productRepository = new Mock<IProductRepository>();
            _productService = new ProductService(_productRepository.Object, new ProductValidator(_productRepository.Object));
        }

        [Test]
        public async Task A_Valid_Product_Can_Be_Created()
        {
            _productRepository.Setup(pr => pr.ExistsAsync(It.IsAny<string>())).ReturnsAsync(true);

            var product = new Product
            {
                Sku = "ABC-123",
                Name = "Simple Test Product",
                Price = 100m
            };

            var result = await _productService.AddAsync(product);
            _productRepository.Verify(pr => pr.AddAsync(product), Times.Once);
        }

        [Test]
        public async Task A_Product_Can_Be_Found_By_Id()
        {
            var guid = Guid.NewGuid();
            _productRepository.Setup(pr => pr.GetAsync(It.IsAny<Guid>())).ReturnsAsync(new Product());
            var result = await _productService.GetAsync(guid);

            _productRepository.Verify(pr => pr.GetAsync(guid), Times.Once);
        }

        [Test]
        public async Task A_Product_Can_Be_Found_By_Sku_Async()
        {
            _productRepository.Setup(pr => pr.GetAsync(It.IsAny<string>())).ReturnsAsync(new Product());
            var result = await _productService.GetAsync("ABC-123");

            _productRepository.Verify(pr => pr.GetAsync("ABC-123"), Times.Once);
        }

        [Test]
        public async Task Getting_All_Products_Async_Returns_Results()
        {
            _productRepository.Setup(pr => pr.GetAllAsync(5, 0))
                .ReturnsAsync(new PagedList<Product>(new List<Product>(), 5, 1, 10));
            var result = await _productService.GetAllAsync(5, 0);

            _productRepository.Verify(pr => pr.GetAllAsync(5, 0), Times.Once);
        }

        [Test]
        public async Task Getting_All_Products_By_SupplierId_Async_Returns_First_Page_Only()
        {
            var guid = Guid.NewGuid();
            _productRepository.Setup(pr => pr.GetAllBySupplierAsync(guid, 5, 0))
                .ReturnsAsync(new PagedList<Product>(new List<Product>(), 5, 1, 10));

            var result = await _productService.GetAllBySupplierAsync(guid, 5, 0);
            _productRepository.Verify(pr => pr.GetAllBySupplierAsync(guid, 5, 0), Times.Once);
        }

        [Test]
        public async Task Getting_All_Products_By_BrandId_Async_Returns_First_Page_Only()
        {
            var guid = Guid.NewGuid();
            _productRepository.Setup(pr => pr.GetAllByBrandAsync(guid, 5, 0))
                .ReturnsAsync(new PagedList<Product>(new List<Product>(), 5, 1, 10));

            var result = await _productService.GetAllByBrandAsync(guid, 5, 0);
            _productRepository.Verify(pr => pr.GetAllByBrandAsync(guid, 5, 0), Times.Once);
        }

        [Test]
        public async Task Product_Sku_Can_Be_Verified_As_Unique_Async()
        {
            _productRepository.Setup(pr => pr.ExistsAsync(It.IsAny<string>())).ReturnsAsync(true);

            var result = await _productService.IsSkuUniqueAsync("ABC-123");
            _productRepository.Verify(pr => pr.ExistsAsync(It.IsAny<string>()), Times.Once);
        }
    }
}
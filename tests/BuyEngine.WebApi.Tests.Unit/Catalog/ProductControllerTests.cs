using BuyEngine.Catalog;
using BuyEngine.WebApi.Catalog;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace BuyEngine.WebApi.Tests.Unit.Catalog
{
    public class ProductControllerTests
    {

        private ProductController _controller;
        private Mock<IProductService> _productService;

        [SetUp]
        public void Setup()
        {
            _productService = new Mock<IProductService>();

            _controller = new ProductController(_productService.Object);
        }

        [Test]
        public void Getting_A_Valid_Product_ById_Returns_Ok()
        {
            _productService.Setup(ps => ps.GetAsync(1)).ReturnsAsync(new Product() {Id = 1});

            var result = _controller.Get(1).Result;
            Assert.That(result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public void Getting_An_Invalid_Product_ById_Returns_BadRequest()
        {
            var result = _controller.Get(0).Result;
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public void Getting_An_Unknown_Product_ById_Returns_NotFound()
        {
            _productService.Setup(ps => ps.GetAsync(1)).ReturnsAsync(null as Product);
            var result = _controller.Get(123).Result;
            Assert.That(result, Is.TypeOf<NotFoundObjectResult>());
        }

        [Test]
        public void Getting_A_Valid_Product_BySku_Returns_Ok()
        {
            const string skuValue = "sku";
            _productService.Setup(ps => ps.GetAsync(skuValue)).ReturnsAsync(new Product() { Id = 1, Sku = skuValue });

            var result = _controller.Get(skuValue).Result;
            Assert.That(result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public void Getting_An_Invalid_Product_BySku_Returns_BadRequest()
        {
            var result = _controller.Get(string.Empty).Result;
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public void Getting_An_Unknown_Product_BySku_Returns_NotFound()
        {
            const string skuValue = "sku";
            _productService.Setup(ps => ps.GetAsync(skuValue)).ReturnsAsync(null as Product);
            var result = _controller.Get(123).Result;
            Assert.That(result, Is.TypeOf<NotFoundObjectResult>());
        }
    }
}
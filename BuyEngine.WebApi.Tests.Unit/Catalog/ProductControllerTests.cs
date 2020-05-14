using BuyEngine.Catalog;
using BuyEngine.WebApi.Catalog;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace BuyEngine.WebApi.Tests.Unit.Catalog
{
    public class ProductControllerTests
    {

        private ProductController _controller;
        private Mock<IProductService> _productService;
        private Mock<ILogger> _logger;


        [SetUp]
        public void Setup()
        {
            _productService = new Mock<IProductService>();
            _logger = new Mock<ILogger>();

            _controller = new ProductController(_productService.Object, _logger.Object);
        }

        [Test]
        public void A_Valid_Product_Returns_Ok()
        {
            _productService.Setup(ps => ps.GetAsync(1)).ReturnsAsync(new Product() {Id = 1});

            var result = _controller.Get(1).Result;
            Assert.That(result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public void A_Invalid_Product_Returns_BadRequest()
        {
            var result = _controller.Get(0).Result;
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public void An_Unknown_ProductId_Returns_NotFound()
        {
            _productService.Setup(ps => ps.GetAsync(1)).ReturnsAsync(null as Product);
            var result = _controller.Get(123).Result;
            Assert.That(result, Is.TypeOf<NotFoundObjectResult>());
        }
    }
}
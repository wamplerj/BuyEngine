using BuyEngine.Catalog;
using BuyEngine.WebApi.Catalog;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;

namespace BuyEngine.WebApi.Tests.Unit.Catalog;

public class ProductControllerTests
{
    private ProductController _controller;
    private Mock<IProductService> _productService;
    private Mock<ILogger<ProductController>> _logger;

    [SetUp]
    public void Setup()
    {
        _productService = new Mock<IProductService>();
        _logger = new Mock<ILogger<ProductController>>();

        _controller = new ProductController(_productService.Object, _logger.Object);
    }

    [Test]
    public void Getting_A_Valid_Product_ById_Returns_Ok()
    {
        var id = Guid.NewGuid();
        _productService.Setup(ps => ps.GetAsync(id)).ReturnsAsync(new Product { Id = id });

        var result = _controller.Get(id).Result;
        Assert.That(result, Is.TypeOf<OkObjectResult>());
    }

    [Test]
    public void Getting_An_Invalid_Product_ById_Returns_BadRequest()
    {
        var result = _controller.Get(Guid.Empty).Result;
        Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
    }

    [Test]
    public void Getting_An_Unknown_Product_ById_Returns_NotFound()
    {
        var id = Guid.NewGuid();
        _productService.Setup(ps => ps.GetAsync(id)).ReturnsAsync(null as Product);
        var result = _controller.Get(id).Result;
        Assert.That(result, Is.TypeOf<NotFoundObjectResult>());
    }

    [Test]
    public void Getting_A_Valid_Product_BySku_Returns_Ok()
    {
        const string skuValue = "sku";
        _productService.Setup(ps => ps.GetAsync(skuValue))
            .ReturnsAsync(new Product { Id = Guid.NewGuid(), Sku = skuValue });

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
        var result = _controller.Get(skuValue).Result;
        Assert.That(result, Is.TypeOf<NotFoundObjectResult>());
    }
}
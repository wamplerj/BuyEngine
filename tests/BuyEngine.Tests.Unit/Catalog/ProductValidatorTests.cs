﻿using BuyEngine.Catalog;
using Moq;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace BuyEngine.Tests.Unit.Catalog;

public class ProductValidatorTests
{
    private Mock<IProductRepository> _productRepository;
    private ProductValidator _validator;

    [SetUp]
    public void Setup()
    {
        _productRepository = new Mock<IProductRepository>();

        _validator = new ProductValidator(_productRepository.Object);
    }

    [Test]
    public async Task A_Valid_Product_Returns_IsValid_And_Has_No_Messages()
    {
        _productRepository.Setup(pr => pr.ExistsAsync(It.IsAny<string>())).ReturnsAsync(false);

        var product = new Product
        {
            Sku = "ABC-123",
            Name = "Test Product",
            Price = 1.0m,
            Quantity = 1
        };


        var result = await _validator.ValidateAsync(product);
        Assert.That(result.IsValid, Is.True);
        Assert.That(result.Messages, Is.Empty);
    }

    [Test]
    public async Task A_Product_With_No_Sku_Fails_Validation()
    {
        _productRepository.Setup(pr => pr.ExistsAsync(It.IsAny<string>())).ReturnsAsync(true);

        var product = new Product
        {
            Name = "Test Product"
        };

        var result = await _validator.ValidateAsync(product);
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Messages.Count, Is.EqualTo(2));
        Assert.That(result.Messages.First().Key, Is.EqualTo(nameof(product.Sku)));
    }

    [Test]
    public async Task A_Disabled_Product_With_No_Sku_Passes_Validation()
    {
        _productRepository.Setup(pr => pr.ExistsAsync(It.IsAny<string>())).ReturnsAsync(true);

        var product = new Product
        {
            Name = "Test Product",
            Enabled = false
        };

        var result = await _validator.ValidateAsync(product);
        Assert.That(result.IsValid, Is.True);
        Assert.That(result.Messages.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task A_Product_Sku_Can_Be_Verified_Unique()
    {
        _productRepository.Setup(pr => pr.ExistsAsync(It.IsAny<string>())).ReturnsAsync(true);

        var result = await _validator.IsSkuUniqueAsync("sku");
        _productRepository.Verify(pr => pr.ExistsAsync("sku"), Times.Once);
    }
}
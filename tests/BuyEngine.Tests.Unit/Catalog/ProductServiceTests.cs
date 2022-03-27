using BuyEngine.Catalog;
using BuyEngine.Common;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuyEngine.Tests.Unit.Catalog;

public class ProductServiceTests
{
    private Mock<ILogger<ProductService>> _logger;
    private Mock<IProductRepository> _productRepository;
    private ProductService _productService;

    [SetUp]
    public void Setup()
    {
        _productRepository = new Mock<IProductRepository>();
        _logger = new Mock<ILogger<ProductService>>();
        _productService = new ProductService(_productRepository.Object, new ProductValidator(_productRepository.Object), _logger.Object);
    }

    [Test]
    public async Task A_Valid_Product_Can_Be_Created()
    {
        _productRepository.Setup(pr => pr.ExistsAsync(It.IsAny<string>())).ReturnsAsync(false);

        var product = new Product
        {
            Sku = "ABC-123",
            Name = "Simple Test Product",
            Price = 100m
        };

        var result = await _productService.AddAsync(product);
        _productRepository.Verify(pr => pr.AddAsync(product), Times.Once);
        Assert.That(product.ToString(), Is.EqualTo("Product: 00000000-0000-0000-0000-000000000000 - ABC-123"));
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
        var result = await _productService.GetAllAsync(5);

        _productRepository.Verify(pr => pr.GetAllAsync(5, 0), Times.Once);
    }

    [Test]
    public async Task Getting_All_Products_By_SupplierId_Async_Returns_First_Page_Only()
    {
        var guid = Guid.NewGuid();
        _productRepository.Setup(pr => pr.GetAllBySupplierAsync(guid, 5, 0))
            .ReturnsAsync(new PagedList<Product>(new List<Product>(), 5, 1, 10));

        var result = await _productService.GetAllBySupplierAsync(guid, 5);
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

    [Test]
    public async Task Product_That_Does_Not_Exist_Can_Not_Be_Removed()
    {
        _productRepository.Setup(pr => pr.GetAsync(It.IsAny<Guid>())).ReturnsAsync(null as Product);

        Assert.ThrowsAsync<ArgumentException>(() => _productService.RemoveAsync(Guid.NewGuid()));
    }

    [Test]
    public async Task Product_With_No_Inventory_Can_Be_Removed_By_Id()
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Sku = "TST-123",
            Name = "Test 123",
            Enabled = true,
            Quantity = 0
        };

        _productRepository.Setup(pr => pr.GetAsync(product.Id)).ReturnsAsync(product);

        await _productService.RemoveAsync(product.Id);

        _productRepository.Verify(pr => pr.RemoveAsync(product), Times.Once);
    }

    [Test]
    public async Task Product_With_Inventory_Can_Be_Not_Removed_By_Id_But_Is_Disabled()
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Sku = "TST-123",
            Name = "Test 123",
            Enabled = true,
            Quantity = 10
        };

        _productRepository.Setup(pr => pr.GetAsync(product.Id)).ReturnsAsync(product);

        await _productService.RemoveAsync(product.Id);

        _productRepository.Verify(pr => pr.RemoveAsync(product), Times.Never);
        _productRepository.Verify(pr => pr.UpdateAsync(product), Times.Once);

        Assert.That(product.Sku, Is.Empty);
        Assert.That(product.Enabled, Is.False);
    }

    [Test]
    public async Task A_Product_Can_Be_Validated_Before_Being_Saved()
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Sku = "TST-123",
            Name = "Test 123",
            Enabled = true,
            Quantity = 10
        };

        _productRepository.Setup(pr => pr.ExistsAsync(It.IsAny<string>())).ReturnsAsync(false);

        var result = await _productService.ValidateAsync(product);
        Assert.That(result.IsValid, Is.True);

        _productRepository.Verify(pr => pr.ExistsAsync(product.Sku), Times.Once);
    }
}
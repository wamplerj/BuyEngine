using BuyEngine.Catalog;
using BuyEngine.Checkout;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyEngine.Tests.Unit.Checkout;

public class CartFeature
{
    private Mock<ICartRepository> _cartRepository;
    private Mock<ILogger<CartService>> _logger;
    private Product _product;
    private Mock<IProductRepository> _productRepository;
    private CartService _service;

    [SetUp]
    public async Task Setup()
    {
        _product = new Product
        {
            Id = Guid.NewGuid(), Name = "My product", Sku = "PRD-001", Enabled = true, Price = 19.95m, Quantity = 24
        };

        _cartRepository = new Mock<ICartRepository>();
        _productRepository = new Mock<IProductRepository>();
        _productRepository.Setup(pr => pr.GetAsync(It.IsAny<Guid>())).ReturnsAsync(_product);

        _logger = new Mock<ILogger<CartService>>();
        _service = new CartService(_cartRepository.Object, _productRepository.Object, new CartValidator(), _logger.Object);
    }

    [Test]
    public async Task AsAShopper_WhenIAddAnItemToACartForTheFirstTime_ACartIsCreated()
    {
        var cartId = Guid.NewGuid();
        var now = DateTime.UtcNow;

        var result = await _service.AddItemAsync(cartId, _product.Id, 5);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(cartId));
        Assert.That(result.Items.Count, Is.EqualTo(1));
        Assert.That(result.Created, Is.GreaterThanOrEqualTo(now));
        Assert.That(result.Expires, Is.GreaterThanOrEqualTo(now.AddHours(2)));

        var item = result.Items.First();

        Assert.That(item.Sku, Is.EqualTo("PRD-001"));
        Assert.That(item.Name, Is.EqualTo("My product"));
        Assert.That(item.Price, Is.EqualTo(19.95m));
        Assert.That(item.Quantity, Is.EqualTo(5));
        Assert.That(item.Total, Is.EqualTo(19.95m * 5));
    }

    [Test]
    public async Task AsAShopper_WhenIAddAnItemToAnExistingCart_TheCartIsUpdated()
    {
        var cartId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        var now = DateTime.UtcNow;

        var cart = new Cart
        {
            Id = cartId,
            Expires = now.AddHours(2),
            Created = now,
            Items = new List<CartItem>
            {
                new() { Id = Guid.NewGuid(), Name = "First", Price = 0.99m, Quantity = 25, Sku = "FST-001" }
            }
        };

        _cartRepository.Setup(cr => cr.GetAsync(cartId)).ReturnsAsync(cart);

        var result = await _service.AddItemAsync(cartId, productId, 5);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(cartId));
        Assert.That(result.Items.Count, Is.EqualTo(2));
        Assert.That(result.Total, Is.EqualTo(124.50));
        Assert.That(result.Created, Is.EqualTo(now));
        Assert.That(result.Expires, Is.GreaterThanOrEqualTo(now.AddHours(2)));

        var item = result.Items[1];

        Assert.That(item.Sku, Is.EqualTo("PRD-001"));
        Assert.That(item.Name, Is.EqualTo("My product"));
        Assert.That(item.Price, Is.EqualTo(19.95m));
        Assert.That(item.Quantity, Is.EqualTo(5));
        Assert.That(item.Total, Is.EqualTo(19.95m * 5));
    }

    [Test]
    public async Task AsAShopper_WhenIAddAnExistingItemToAnExistingCart_TheExistingItemIsUpdated()
    {
        var cartId = Guid.NewGuid();

        var cart = new Cart
        {
            Id = cartId,
            Expires = DateTime.UtcNow.AddHours(2),
            Items = new List<CartItem>
            {
                new() { Id = Guid.NewGuid(), ProductId = _product.Id, Name = "My product", Price = 19.95m, Quantity = 3, Sku = "PRD-001" }
            }
        };

        _cartRepository.Setup(cr => cr.GetAsync(cartId)).ReturnsAsync(cart);

        var result = await _service.AddItemAsync(cartId, _product.Id, 5);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(cartId));
        Assert.That(result.Items.Count, Is.EqualTo(1));

        var item = result.Items[0];

        Assert.That(item.Sku, Is.EqualTo("PRD-001"));
        Assert.That(item.Name, Is.EqualTo("My product"));
        Assert.That(item.Price, Is.EqualTo(19.95m));
        Assert.That(item.Quantity, Is.EqualTo(5));
        Assert.That(item.Total, Is.EqualTo(19.95m * 5));
    }

    [Test]
    public async Task WhenACartIsAbandonedAndExpired_ItIsRemovedFromTheRepository()
    {
        var cart = new Cart
        {
            Id = Guid.NewGuid(),
            Expires = DateTime.UtcNow.AddHours(-1), //Expired
            Items = new List<CartItem>
            {
                new() { Id = Guid.NewGuid(), ProductId = _product.Id, Name = "My product", Price = 19.95m, Quantity = 3, Sku = "PRD-001" }
            }
        };

        _cartRepository.Setup(cr => cr.GetAsync(cart.Id)).ReturnsAsync(cart);
        _cartRepository.Setup(cr => cr.Delete(It.IsAny<Guid>())).ReturnsAsync(true);

        var result = await _service.AbandonAsync(cart.Id);

        Assert.That(result, Is.True);
        _cartRepository.Verify(cr => cr.GetAsync(It.IsAny<Guid>()), Times.Once);
        _cartRepository.Verify(cr => cr.Delete(It.IsAny<Guid>()), Times.Once);
    }
}
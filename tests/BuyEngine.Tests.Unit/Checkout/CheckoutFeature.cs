using BuyEngine.Checkout;
using BuyEngine.Checkout.Payment;
using BuyEngine.Checkout.Shipping;
using BuyEngine.Customer;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuyEngine.Tests.Unit.Checkout;

public class CheckoutFeature
{
    private SalesOrderBuilder _builder;
    private Mock<IInventoryService> _inventoryService;
    private Mock<ILogger<CheckoutService>> _logger;
    private Mock<IOrderService> _orderService;
    private Mock<IPaymentService> _paymentService;
    private Mock<IShippingService> _shippingService;

    [Test]
    public async Task A_Shopper_Can_Checkout()
    {
        var cart = new Cart
        {
            Created = DateTime.UtcNow.AddMinutes(-5),
            Expires = DateTime.UtcNow.AddMinutes(240),
            Items = new List<CartItem>
            {
                new() { Id = Guid.NewGuid(), Name = "Test Product", Price = 29.95m, Quantity = 3 }
            }
        };

        var billTo = new SalesOrderAddress
        {
            Customer = "Some Name",
            EmailAddress = "blah@blah.com",
            PhoneNumber = "5555551212",
            Line1 = "123 Some St",
            City = "SomeVille",
            StateProvince = "WA",
            PostalCode = "98101",
            CountryRegion = "USA"
        };
        var shipTo = new SalesOrderAddress
        {
            Customer = "Some Name",
            EmailAddress = "blah@blah.com",
            PhoneNumber = "5555551212",
            Line1 = "123 Some St",
            City = "SomeVille",
            StateProvince = "WA",
            PostalCode = "98101",
            CountryRegion = "USA"
        };
        var shippingMethod = new ShippingMethod
        {
            Name = "Fedex 2 Day Shipping",
            Price = 9.95m,
            Timeframe = "2 Business Days"
        };
        var payment = new PaymentInformation
        {
            CreditCardNumber = "8111111111111111",
            Ccv = "123",
            Expiration = DateTime.Today.AddYears(5),
            PostalCode = "98101",
            Payee = "Testy McTesterson"
        };

        var salesOrder = await _builder
            .SetCart(cart)
            .SetBillTo(billTo)
            .SetShipTo(shipTo)
            .SetShippingMethod(shippingMethod)
            .SetPayment(payment)
            .BuildAsync();

        var checkoutService = new CheckoutService(null,
            _inventoryService.Object,
            _shippingService.Object,
            _paymentService.Object,
            _orderService.Object,
            _logger.Object);

        var orderId = await checkoutService.CheckoutAsync(salesOrder);

        _inventoryService.Verify(ii => ii.IsAvailable(It.IsAny<IEnumerable<string>>()), Times.Once);
        _shippingService.Verify(ss => ss.IsShippingAvailable(It.IsAny<ShippingMethod>(), shipTo), Times.Once);
        _paymentService.Verify(ps => ps.IsAuthValidAsync(payment), Times.Once);
        _paymentService.Verify(ps => ps.CollectPaymentAsync(payment), Times.Once);

        Assert.That(orderId, Is.Not.EqualTo(Guid.Empty));
    }

    [SetUp]
    public async Task Setup()
    {
        _builder = new SalesOrderBuilder(new CartValidator(),
            new SalesOrderAddressValidator(new AddressValidator()),
            new ShippingValidator(),
            new PaymentValidator());

        _shippingService = new Mock<IShippingService>();
        _shippingService.Setup(ss =>
                ss.IsShippingAvailable(It.IsAny<ShippingMethod>(), It.IsAny<Address>()))
            .ReturnsAsync(true);

        var products = new List<ProductInventory>
        {
            new() { OutOfStock = false },
            new() { OutOfStock = false },
            new() { OutOfStock = false }
        };

        _inventoryService = new Mock<IInventoryService>();
        _inventoryService.Setup(i => i.IsAvailable(It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(new InventoryStatus(products));

        _paymentService = new Mock<IPaymentService>();
        _paymentService.Setup(ps => ps.IsAuthValidAsync(It.IsAny<PaymentInformation>())).ReturnsAsync(true);
        _paymentService.Setup(ps => ps.CollectPaymentAsync(It.IsAny<PaymentInformation>()))
            .ReturnsAsync(Guid.NewGuid());

        _orderService = new Mock<IOrderService>();
        _orderService.Setup(os => os.AddAsync(It.IsAny<Order>())).ReturnsAsync(Guid.NewGuid());

        _logger = new Mock<ILogger<CheckoutService>>();
    }
}
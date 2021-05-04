using System;
using System.Linq;
using BuyEngine.Checkout;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using BuyEngine.Catalog;
using Microsoft.Extensions.Logging;

namespace BuyEngine.Tests.Unit.Checkout
{
    public class CartFeature
    {

        [Test]
        public async Task AsAShopper_WhenIAddAnItemToACartForTheFirstTime_ACartIsCreated()
        {
            var cartRepository = new Mock<ICartRepository>();
            //cartRepository.Setup(cr => cr.GetAsync(cartId:))

            var productRepository = new Mock<IProductRepository>();
            productRepository.Setup(pr => pr.GetAsync(It.IsAny<Guid>())).ReturnsAsync(new Product
                {Name = "My product", Sku = "PRD-001", Enabled = true, Price = 19.95m, Quantity = 24});

            var logger = new Mock<ILogger<CartService>>();

            var service = new CartService(cartRepository.Object, productRepository.Object, new CartValidator(), logger.Object);
            var cartId = Guid.NewGuid();
            var productId = Guid.NewGuid();

            var result = await service.AddItemAsync(cartId, productId, 5);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(cartId));

            var item = result.Items.First();

            Assert.That(item.Sku, Is.EqualTo("PRD-001"));
            Assert.That(item.Name, Is.EqualTo("My product"));
            Assert.That(item.Price, Is.EqualTo(19.95m));
            Assert.That(item.Quantity, Is.EqualTo(5));
            Assert.That(item.Total, Is.EqualTo(19.95m * 5));
        }

    }
}

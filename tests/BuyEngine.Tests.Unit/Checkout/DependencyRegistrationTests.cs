using BuyEngine.Catalog;
using BuyEngine.Checkout;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Threading.Tasks;

namespace BuyEngine.Tests.Unit.Checkout;

public class DependencyRegistrationTests
{
    [Test]
    public async Task Checkout_Dependencies_Resolve()
    {
        var services = new ServiceCollection();

        services.AddLogging();
        services.AddCatalogServices(true);
        services.AddCheckoutServices(true);

        var provider = services.BuildServiceProvider();

        var cartService = provider.GetService<ICartService>();
        Assert.That(cartService, Is.Not.Null);
        Assert.That(cartService, Is.TypeOf<CartService>());

        var checkoutService = provider.GetService<ICheckoutService>();
        Assert.That(checkoutService, Is.Not.Null);
        Assert.That(checkoutService, Is.TypeOf<CheckoutService>());
    }
}
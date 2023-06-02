using BuyEngine.Customer;

namespace BuyEngine.Checkout.Shipping;

public class ShippingService : IShippingService
{
    private readonly ShippingConfiguration _configuration;

    public ShippingService(ShippingConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<List<ShippingMethod>> GetAvailableShippingMethods(Address address) =>
        new()
        {
            new() { Name = "In-Store Pickup", Price = 0.00m, Timeframe = "2 Hours" }
        };

    public async Task<decimal> GetPackageWeight(Cart cart)
    {
        var weight = cart.Items.Sum(ci => ci.ItemWeight * ci.Quantity);
        return weight;
    }

    public async Task<PackageDimension> GetPackageDimensions(Cart cart) => new PackageDimension(1, 1, 1, UnitOfMeasure.Inches);

    public async Task<Guid> BuyShipping(Cart cart, Address address, decimal weight, PackageDimension dimensions) => Guid.NewGuid();

    public Task<bool> IsShippingAvailable(ShippingMethod shippingMethod, Address address) => throw new NotImplementedException();
}

public class ShippingConfiguration
{
}

public interface IShippingService
{
    Task<List<ShippingMethod>> GetAvailableShippingMethods(Address address);
    Task<decimal> GetPackageWeight(Cart cart);
    Task<PackageDimension> GetPackageDimensions(Cart cart);
    Task<Guid> BuyShipping(Cart cart, Address address, decimal weight, PackageDimension dimensions);
    Task<bool> IsShippingAvailable(ShippingMethod shippingMethod, Address address);
}
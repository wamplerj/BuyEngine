namespace BuyEngine.Checkout;

public class InventoryService : IInventoryService
{
    public Task<InventoryStatus> IsAvailable(IEnumerable<string> skus) => throw new NotImplementedException();
}

public interface IInventoryService
{
    Task<InventoryStatus> IsAvailable(IEnumerable<string> skus);
}

public class InventoryStatus
{
    private readonly ICollection<ProductInventory> _products;

    public bool AllAvailable => _products.Any(p => !p.OutOfStock);

    public InventoryStatus(ICollection<ProductInventory> products)
    {
        _products = products;
    }
}

public class ProductInventory
{
    public bool OutOfStock { get; set; }
}
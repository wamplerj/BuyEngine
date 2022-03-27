using BuyEngine.Catalog.Brands;
using BuyEngine.Catalog.Suppliers;

namespace BuyEngine.Catalog;

internal class InMemoryDataStore : ICatalogDataStore
{
    public List<Product> Products { get; } = new();
    public List<Brand> Brands { get; } = new();
    public List<Supplier> Suppliers { get; } = new();
}

public interface ICatalogDataStore
{
    List<Product> Products { get; }
    List<Brand> Brands { get; }
    List<Supplier> Suppliers { get; }
}
using BuyEngine.Common;
using Microsoft.Extensions.Logging;

namespace BuyEngine.Catalog;

internal class InMemoryProductRepository : IProductRepository
{
    private readonly ILogger<InMemoryProductRepository> _logger;
    private readonly List<Product> _products;

    public InMemoryProductRepository(ICatalogDataStore dataStore, ILogger<InMemoryProductRepository> logger)
    {
        _products = dataStore.Products;
        _logger = logger;
    }

    public async Task<Product> GetAsync(Guid productId)
    {
        if (productId == default)
            return default;

        return _products.FirstOrDefault(p => p.Id == productId);
    }

    public async Task<Product> GetAsync(string sku)
    {
        if (string.IsNullOrWhiteSpace(sku))
            return default;

        return _products.FirstOrDefault(p => p.Sku.Equals(sku, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<PagedList<Product>> GetAllAsync(int pageSize, int page)
    {
        var skipCount = page.SkipCount(pageSize);
        skipCount = skipCount < pageSize ? 0 : skipCount;

        return _products.Skip(skipCount).Take(pageSize).ToPagedList(pageSize, page, _products.Count);
    }

    public async Task<PagedList<Product>> GetAllBySupplierAsync(Guid supplierId, int pageSize, int page)
    {
        var skipCount = page.SkipCount(pageSize);
        var products = _products
            .Where(p => p.Supplier.Id == supplierId)
            .Skip(skipCount)
            .Take(pageSize)
            .ToPagedList(pageSize, page, _products.Count);

        return products;
    }

    public async Task<PagedList<Product>> GetAllByBrandAsync(Guid brandId, int pageSize, int page)
    {
        var skipCount = page.SkipCount(pageSize);
        var products = _products
            .Where(p => p.Brand.Id == brandId)
            .Skip(skipCount)
            .Take(pageSize)
            .ToList();

        return products.ToPagedList(pageSize, page, _products.Count);
    }

    public async Task<Guid> AddAsync(Product product)
    {
        var exists = _products.Any(p => p.Sku.Equals(product.Sku, StringComparison.OrdinalIgnoreCase));
        if (exists)
            throw new ArgumentException($"Product Sku: {product.Sku} already exists");

        product.Id = Guid.NewGuid();
        _products.Add(product);

        return product.Id;
    }

    public async Task<bool> UpdateAsync(Product product)
    {
        var index = _products.FindIndex(p => p.Sku.Equals(product.Sku, StringComparison.OrdinalIgnoreCase));
        if (index == -1) return false;

        _products[index] = product;
        return true;
    }

    public async Task RemoveAsync(Product product)
    {
        var success = _products.Remove(product);
    }

    public async Task<bool> ExistsAsync(string sku) => _products.Any(p => p.Sku.Equals(sku, StringComparison.OrdinalIgnoreCase));
}
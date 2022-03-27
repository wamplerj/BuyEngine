using BuyEngine.Common;
using Microsoft.Extensions.Logging;

namespace BuyEngine.Catalog.Suppliers;

internal class InMemorySupplierRepository : ISupplierRepository
{
    private readonly ILogger<InMemorySupplierRepository> _logger;
    private readonly List<Supplier> _suppliers;

    public InMemorySupplierRepository(ICatalogDataStore dataStore, ILogger<InMemorySupplierRepository> logger)
    {
        _logger = logger;
        _suppliers = dataStore.Suppliers;
    }

    public async Task<Supplier> GetAsync(Guid supplierId)
    {
        if (supplierId == default)
            return default;

        return _suppliers.FirstOrDefault(p => p.Id == supplierId);
    }

    public async Task<IPagedList<Supplier>> GetAllAsync(int pageSize, int page)
    {
        var skipCount = page.SkipCount(pageSize);
        skipCount = skipCount < pageSize ? 0 : skipCount;

        return _suppliers.Skip(skipCount).Take(pageSize).ToPagedList(pageSize, page, _suppliers.Count);
    }

    public async Task<Guid> AddAsync(Supplier supplier)
    {
        if (_suppliers.Any(b => b.Id == supplier.Id))
            throw new ArgumentException($"Brand Id: {supplier.Id} already exists");

        supplier.Id = Guid.NewGuid();
        _suppliers.Add(supplier);

        return supplier.Id;
    }

    public async Task<bool> UpdateAsync(Supplier supplier)
    {
        var index = _suppliers.FindIndex(p => p.Id == supplier.Id);
        if (index == -1) return false;

        _suppliers[index] = supplier;
        return true;
    }

    public async Task RemoveAsync(Supplier supplier)
    {
        var success = _suppliers.Remove(supplier);
    }

    public async Task RemoveAsync(Guid supplierId)
    {
        var supplier = _suppliers.FirstOrDefault(b => b.Id == supplierId);
        if (supplier == default) return;

        _suppliers.Remove(supplier);
    }
}
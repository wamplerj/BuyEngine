using BuyEngine.Common;
using Microsoft.Extensions.Logging;

namespace BuyEngine.Catalog.Brands;

internal class InMemoryBrandRepository : IBrandRepository
{
    private readonly List<Brand> _brands;
    private readonly ILogger<InMemoryBrandRepository> _logger;

    public InMemoryBrandRepository(ICatalogDataStore dataStore, ILogger<InMemoryBrandRepository> logger)
    {
        _brands = dataStore.Brands;
        _logger = logger;
    }

    public async Task<Brand> GetAsync(Guid brandId)
    {
        if (brandId == default)
            return default;

        return _brands.FirstOrDefault(p => p.Id == brandId);
    }

    public async Task<IPagedList<Brand>> GetAllAsync(int pageSize, int page)
    {
        var skipCount = page.SkipCount(pageSize);
        skipCount = skipCount < pageSize ? 0 : skipCount;

        return _brands.Skip(skipCount).Take(pageSize).ToPagedList(pageSize, page, _brands.Count);
    }

    public async Task<Guid> AddAsync(Brand brand)
    {
        if (_brands.Any(b => b.Id == brand.Id))
            throw new ArgumentException($"Brand Id: {brand.Id} already exists");

        brand.Id = Guid.NewGuid();
        _brands.Add(brand);

        return brand.Id;
    }

    public async Task<bool> UpdateAsync(Brand brand)
    {
        var index = _brands.FindIndex(p => p.Id == brand.Id);
        if (index == -1) return false;

        _brands[index] = brand;
        return true;
    }

    public async Task RemoveAsync(Brand brand)
    {
        var success = _brands.Remove(brand);
    }

    public async Task RemoveAsync(Guid brandId)
    {
        var brand = _brands.FirstOrDefault(b => b.Id == brandId);
        if (brand == default) return;

        _brands.Remove(brand);
    }
}
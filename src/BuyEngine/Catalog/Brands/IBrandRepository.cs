namespace BuyEngine.Catalog.Brands;

internal class NullBrandRepository : IBrandRepository
{
    public Task<Brand> GetAsync(Guid brandId) => throw new NotImplementedException();

    public Task<IList<Brand>> GetAllAsync(int pageSize, int page) => throw new NotImplementedException();

    public Task<Guid> AddAsync(Brand brand) => throw new NotImplementedException();

    public Task<bool> UpdateAsync(Brand brand) => throw new NotImplementedException();

    public Task RemoveAsync(Brand brand) => throw new NotImplementedException();

    public Task RemoveAsync(Guid brandId) => throw new NotImplementedException();
}

public interface IBrandRepository
{
    Task<Brand> GetAsync(Guid brandId);
    Task<IList<Brand>> GetAllAsync(int pageSize, int page);
    Task<Guid> AddAsync(Brand brand);
    Task<bool> UpdateAsync(Brand brand);
    Task RemoveAsync(Brand brand);
    Task RemoveAsync(Guid brandId);
}
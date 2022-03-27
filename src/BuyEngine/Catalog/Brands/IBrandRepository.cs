using BuyEngine.Common;

namespace BuyEngine.Catalog.Brands;

public interface IBrandRepository
{
    Task<Brand> GetAsync(Guid brandId);
    Task<IPagedList<Brand>> GetAllAsync(int pageSize, int page);
    Task<Guid> AddAsync(Brand brand);
    Task<bool> UpdateAsync(Brand brand);
    Task RemoveAsync(Brand brand);
    Task RemoveAsync(Guid brandId);
}
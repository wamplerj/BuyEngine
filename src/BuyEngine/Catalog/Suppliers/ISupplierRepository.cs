using BuyEngine.Common;

namespace BuyEngine.Catalog.Suppliers;

public interface ISupplierRepository
{
    Task<Supplier> GetAsync(Guid brandId);
    Task<IPagedList<Supplier>> GetAllAsync(int pageSize, int page);
    Task<Guid> AddAsync(Supplier brand);
    Task<bool> UpdateAsync(Supplier brand);
    Task RemoveAsync(Supplier brand);
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuyEngine.Catalog.Suppliers
{
    public interface ISupplierRepository
    {
        Task<Supplier> GetAsync(Guid brandId);
        Task<IList<Supplier>> GetAllAsync(int pageSize, int page);
        Task<int> AddAsync(Supplier brand);
        Task<bool> UpdateAsync(Supplier brand);
        Task RemoveAsync(Supplier brand);
    }
}

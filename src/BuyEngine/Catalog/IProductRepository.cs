using BuyEngine.Common;
using System;
using System.Threading.Tasks;

namespace BuyEngine.Catalog
{
    public interface IProductRepository
    {
        Task<Product> GetAsync(Guid productId);
        Task<Product> GetAsync(string sku);
        Task<PagedList<Product>> GetAllAsync(int pageSize, int page);
        Task<PagedList<Product>> GetAllBySupplierAsync(Guid supplierId, int pageSize, int page);
        Task<PagedList<Product>> GetAllByBrandAsync(Guid brandId, int pageSize, int page);

        Task<int> AddAsync(Product product);
        Task<bool> UpdateAsync(Product product);
        Task RemoveAsync(Product product);
        Task<bool> ExistsAsync(string sku);
    }
}

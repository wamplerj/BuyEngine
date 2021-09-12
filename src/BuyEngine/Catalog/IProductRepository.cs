using BuyEngine.Common;
using System;
using System.Threading.Tasks;

namespace BuyEngine.Catalog
{
    public class InMemoryProductRepository : IProductRepository
    {
        public Task<Product> GetAsync(Guid productId)
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetAsync(string sku)
        {
            throw new NotImplementedException();
        }

        public Task<PagedList<Product>> GetAllAsync(int pageSize, int page)
        {
            throw new NotImplementedException();
        }

        public Task<PagedList<Product>> GetAllBySupplierAsync(Guid supplierId, int pageSize, int page)
        {
            throw new NotImplementedException();
        }

        public Task<PagedList<Product>> GetAllByBrandAsync(Guid brandId, int pageSize, int page)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddAsync(Product product)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Product product)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(Product product)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(string sku)
        {
            throw new NotImplementedException();
        }
    }


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

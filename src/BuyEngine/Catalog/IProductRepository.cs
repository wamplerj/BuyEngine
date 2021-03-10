﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuyEngine.Catalog
{
    public interface IProductRepository
    {
        Task<Product> GetAsync(int productId);
        Task<Product> GetAsync(string sku);
        Task<IList<Product>> GetAllAsync(int pageSize, int page);
        Task<IList<Product>> GetAllBySupplierAsync(int supplierId, int pageSize, int page);
        Task<IList<Product>> GetAllByBrandAsync(int brandId, int pageSize, int page);

        Task<int> AddAsync(Product product);
        Task<bool> UpdateAsync(Product product);
        Task RemoveAsync(Product product);
        Task<bool> ExistsAsync(string sku);
    }
}

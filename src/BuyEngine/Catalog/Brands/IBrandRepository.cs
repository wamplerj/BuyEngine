using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuyEngine.Catalog.Brands
{
    public interface IBrandRepository
    {
        Task<Brand> GetAsync(int brandId);
        Task<IList<Brand>> GetAllAsync(int pageSize, int page);
        Task<int> AddAsync(Brand brand);
        Task<bool> UpdateAsync(Brand brand);
        Task RemoveAsync(Brand brand);
        Task RemoveAsync(int brandId);
    }
}

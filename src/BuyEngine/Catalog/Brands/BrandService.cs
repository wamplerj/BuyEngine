using BuyEngine.Common;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuyEngine.Persistence;

namespace BuyEngine.Catalog.Brands
{
    public class BrandService : IBrandService
    {
        private readonly IStoreDbContext _storeDbContext;
        private readonly IModelValidator<Brand> _validator;

        public BrandService(IStoreDbContext storeDbContext, IModelValidator<Brand> validator)
        {
            _storeDbContext = storeDbContext;
            _validator = validator;
        }

        public Brand Get(int brandId)
        {
            return GetAsync(brandId).Result;
        }

        public async Task<Brand> GetAsync(int brandId)
        {
            return await _storeDbContext.Brands.FindAsync(brandId);
        }

        public IList<Brand> GetAll(int pageSize = CatalogConfiguration.DefaultRecordsPerPage, int page = 0)
        {
            return GetAllAsync(pageSize, page).Result;
        }
        
        public async Task<IList<Brand>> GetAllAsync(int pageSize = CatalogConfiguration.DefaultRecordsPerPage, int page = 0)
        {
            var skip = (page * pageSize);

            return await _storeDbContext.Brands.Skip(skip).Take(pageSize).ToListAsync();
        }

        public int Add(Brand brand)
        {
            Guard.Null(brand, nameof(brand));

            var result = _validator.Validate(brand);
            if (!result.IsValid)
                throw new ValidationException(result, nameof(brand));

            _storeDbContext.Brands.Add(brand);
            _storeDbContext.SaveChanges();
            return brand.Id;
        }

        public void Update(Brand brand)
        {
            Guard.Null(brand, nameof(brand));
            
            var result = _validator.Validate(brand);
            if (!result.IsValid)
                throw new ValidationException(result, nameof(brand));

            _storeDbContext.Brands.Update(brand);
            _storeDbContext.SaveChanges();
        }

        public void Remove(Brand brand)
        {
            Guard.Null(brand, nameof(brand));
            Guard.NegativeOrZero(brand.Id, nameof(brand.Id));
            
            _storeDbContext.Entry(brand).State = EntityState.Deleted;
            _storeDbContext.Brands.Remove(brand);
            _storeDbContext.SaveChanges();
        }

        public void Remove(int brandId)
        {
            Guard.NegativeOrZero(brandId, nameof(brandId));
            
            var brand = new Brand() {Id = brandId };
            Remove(brand);
        }

        public bool IsValid(Brand brand)
        {
            var result = Validate(brand);
            return result.IsValid;
        }

        public ValidationResult Validate(Brand brand)
        {
            return _validator.Validate(brand);
        }
    }

    public interface IBrandService
    {
        Brand Get(int brandId);
        Task<Brand> GetAsync(int brandId);
        IList<Brand> GetAll(int pageSize = 25, int page = 0);
        Task<IList<Brand>> GetAllAsync(int pageSize = 25, int page = 0);
        int Add(Brand brand);
        void Update(Brand brand);
        void Remove(Brand brand);
        void Remove(int brandId);

        bool IsValid(Brand brand);
        ValidationResult Validate(Brand brand);
    }
}

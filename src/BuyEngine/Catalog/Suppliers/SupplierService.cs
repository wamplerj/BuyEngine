using BuyEngine.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyEngine.Catalog.Suppliers
{
    public class SupplierService : ISupplierService
    {
        private readonly ICatalogDbContext _catalogDbContext;
        private readonly IModelValidator<Supplier> _validator;

        public SupplierService(ICatalogDbContext catalogDbContext, IModelValidator<Supplier> validator)
        {
            _catalogDbContext = catalogDbContext;
            _validator = validator;
        }

        public Supplier Get(int supplierId)
        {
            return GetAsync(supplierId).Result;
        }

        public async Task<Supplier> GetAsync(int supplierId)
        {
            return await _catalogDbContext.Suppliers.FindAsync(supplierId);
        }

        public IList<Supplier> GetAll(int pageSize = CatalogConfiguration.DefaultRecordsPerPage, int page = 0)
        {
            return GetAllAsync(pageSize, page).Result;
        }
        
        public async Task<IList<Supplier>> GetAllAsync(int pageSize = CatalogConfiguration.DefaultRecordsPerPage, int page = 0)
        {
            var skip = (page * pageSize);

            return await _catalogDbContext.Suppliers.Skip(skip).Take(pageSize).ToListAsync();
        }

        public int Add(Supplier supplier)
        {
            var result = _validator.Validate(supplier);
            if (!result.IsValid)
                throw new ValidationException(result, nameof(supplier));

            _catalogDbContext.Suppliers.Add(supplier);
            return supplier.Id;
        }

        public void Update(Supplier supplier)
        {
            var result = _validator.Validate(supplier);
            if (!result.IsValid)
                throw new ValidationException(result, nameof(supplier));

            _catalogDbContext.Suppliers.Update(supplier);
        }

        public void Remove(Supplier supplier)
        {
            if (supplier == null)
                throw new ArgumentNullException(nameof(supplier), "Product can not be null");

            if (supplier.Id <= 0)
                throw new ArgumentOutOfRangeException(nameof(supplier.Id), "Supplier.Id must be greater then 0");

            _catalogDbContext.Suppliers.Remove(supplier);
            _catalogDbContext.SaveChanges();
        }

        public void Remove(int supplierId)
        {
            var supplier = new Supplier() {Id = supplierId};
            _catalogDbContext.Entry(supplier).State = EntityState.Deleted;
            Remove(supplier);
        }

        public bool IsValid(Supplier supplier)
        {
            var result = Validate(supplier);
            return result.IsValid;
        }

        public ValidationResult Validate(Supplier supplier)
        {
            return _validator.Validate(supplier);
        }
    }

    public interface ISupplierService
    {
        Supplier Get(int supplierId);
        Task<Supplier> GetAsync(int supplierId);
        IList<Supplier> GetAll(int pageSize = 25, int page = 0);
        Task<IList<Supplier>> GetAllAsync(int pageSize = 25, int page = 0);
        int Add(Supplier supplier);
        void Update(Supplier supplier);
        void Remove(Supplier supplier);
        void Remove(int supplierId);

        bool IsValid(Supplier supplier);
        ValidationResult Validate(Supplier supplier);
    }
}

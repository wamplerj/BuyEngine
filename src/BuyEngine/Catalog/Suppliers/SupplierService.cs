using BuyEngine.Common;

namespace BuyEngine.Catalog.Suppliers
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IModelValidator<Supplier> _validator;

        public SupplierService(ISupplierRepository supplierRepository, IModelValidator<Supplier> validator)
        {
            _supplierRepository = supplierRepository;
            _validator = validator;
        }

        public async Task<Supplier> GetAsync(Guid supplierId)
        {
            return await _supplierRepository.GetAsync(supplierId);
        }
        
        public async Task<IList<Supplier>> GetAllAsync(int pageSize = CatalogConfiguration.DefaultRecordsPerPage, int page = 0)
        {
            return await _supplierRepository.GetAllAsync(pageSize, page);
        }

        public async Task<Guid> AddAsync(Supplier supplier)
        {
            var result = await _validator.ValidateAsync(supplier);
            if (!result.IsValid)
                throw new ValidationException(result, nameof(supplier));

            var id = await _supplierRepository.AddAsync(supplier);
            return id;
        }

        public async Task<bool> UpdateAsync(Supplier supplier)
        {
            var result = await _validator.ValidateAsync(supplier);
            if (!result.IsValid)
                throw new ValidationException(result, nameof(supplier));

            var success = await _supplierRepository.UpdateAsync(supplier);
            return success;
        }

        public async Task RemoveAsync(Supplier supplier)
        {
            if (supplier == null)
                throw new ArgumentNullException(nameof(supplier), "Product can not be null");

            if (supplier.Id == default)
                throw new ArgumentOutOfRangeException(nameof(supplier.Id), "Supplier.Id must be a valid Guid");

            await _supplierRepository.RemoveAsync(supplier);
        }

        public async Task RemoveAsync(Guid supplierId)
        {
            var supplier = await GetAsync(supplierId);
            await RemoveAsync(supplier);
        }

        public async Task<bool> IsValidAsync(Supplier supplier)
        {
            var result = await ValidateAsync(supplier);
            return result.IsValid;
        }

        public async Task<ValidationResult> ValidateAsync(Supplier supplier)
        {
            return await _validator.ValidateAsync(supplier);
        }
    }

    public interface ISupplierService
    {
        Task<Supplier> GetAsync(Guid supplierId);
        Task<IList<Supplier>> GetAllAsync(int pageSize = 25, int page = 0);
        Task<Guid> AddAsync(Supplier supplier);
        Task<bool> UpdateAsync(Supplier supplier);
        Task RemoveAsync(Supplier supplier);
        Task RemoveAsync(Guid supplierId);

        Task<bool> IsValidAsync(Supplier supplier);
        Task<ValidationResult> ValidateAsync(Supplier supplier);
    }
}

using BuyEngine.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuyEngine.Catalog
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductValidator _productValidator;

        public ProductService(IProductRepository productRepository, IProductValidator productValidator)
        {
            _productRepository = productRepository;
            _productValidator = productValidator;
        }

        public async Task<Product> GetAsync(int productId)
        {
            var product = await _productRepository.GetAsync(productId);

            return product;
        }

        public async Task<Product> GetAsync(string sku)
        {
            return await _productRepository.GetAsync(sku);
        }
        
        public async Task<IList<Product>> GetAllAsync(int pageSize = CatalogConfiguration.DefaultRecordsPerPage, int page = 0)
        {
            return await _productRepository.GetAllAsync(pageSize, page);
        }

        public async Task<IList<Product>> GetAllByBrandAsync(int brandId, int pageSize, int page)
        {
            return await _productRepository.GetAllByBrandAsync(brandId, pageSize, page);
        }

        public async Task<IList<Product>> GetAllBySupplierAsync(int supplierId, int pageSize = CatalogConfiguration.DefaultRecordsPerPage, int page = 0)
        {
            return await _productRepository.GetAllBySupplierAsync(supplierId, pageSize, page);
        }

        public async Task<int> AddAsync(Product product)
        {
            Guard.Null(product, nameof(product));

            var result = await _productValidator.ValidateAsync(product);
            if (!result.IsValid)
                throw new ValidationException(result, nameof(product));

            var id = await _productRepository.AddAsync(product);
            return id;
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            Guard.Null(product, nameof(product));

            var result = await _productValidator.ValidateAsync(product, requireUniqueSku:false);
            if (!result.IsValid)
                throw new ValidationException(result, nameof(product));

            var success = await _productRepository.UpdateAsync(product);
            return success;
        }

        public async Task RemoveAsync(Product product)
        {
            Guard.Null(product, nameof(product));
            await _productRepository.RemoveAsync(product);
        }

        public async Task RemoveAsync(int productId)
        {
            Guard.NegativeOrZero(productId, nameof(productId));

            var product = await _productRepository.GetAsync(productId);
            if (product == null)
                throw new ArgumentException("ProductId could not be found", nameof(productId));

            await _productRepository.RemoveAsync(product);
        }

        public Task<bool> IsSkuUniqueAsync(string sku)
        {
            return _productValidator.IsSkuUniqueAsync(sku);
        }

        public async Task<ValidationResult> ValidateAsync(Product product, bool requireUniqueSku)
        {
            return await _productValidator.ValidateAsync(product, requireUniqueSku);
        }
    }

    public interface IProductService
    {
        Task<Product> GetAsync(int productId);
        Task<Product> GetAsync(string sku);
        Task<IList<Product>> GetAllAsync(int pageSize, int page);
        Task<IList<Product>> GetAllByBrandAsync(int brandId, int pageSize, int page);
        Task<IList<Product>> GetAllBySupplierAsync(int supplierId, int pageSize, int page);

        Task<bool> IsSkuUniqueAsync(string sku);
        Task<int> AddAsync(Product product);
        Task RemoveAsync(int productId);
        Task RemoveAsync(Product product);
        Task<bool> UpdateAsync(Product product);
        Task<ValidationResult> ValidateAsync(Product product, bool requireUniqueSku);
        
    }
}
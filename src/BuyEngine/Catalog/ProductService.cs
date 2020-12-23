using BuyEngine.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyEngine.Catalog
{
    public class ProductService : IProductService
    {
        private readonly ICatalogDbContext _catalogDbContext;
        private readonly IProductValidator _productValidator;

        public ProductService(ICatalogDbContext catalogDbContext, IProductValidator productValidator)
        {
            _catalogDbContext = catalogDbContext;
            _productValidator = productValidator;
        }

        public Product Get(int productId)
        {
            return GetAsync(productId).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public async Task<Product> GetAsync(int productId)
        {
            var product = await _catalogDbContext.Products
                .AsNoTracking()
                .Include(s => s.Supplier)
                .Include(b => b.Brand)
                .Where(p => p.Id == productId).FirstOrDefaultAsync();

            return product;
        }

        public Product Get(string sku)
        {
            return GetAsync(sku).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public async Task<Product> GetAsync(string sku)
        {
            return await _catalogDbContext.Products
                .AsNoTracking()
                .Include(s => s.Supplier)
                .Include(b => b.Brand)
                .Where(p => p.Sku.Equals(sku)).FirstOrDefaultAsync();
        }

        public IList<Product> GetAll(int pageSize = CatalogConfiguration.DefaultRecordsPerPage, int page = 0)
        {
            return _catalogDbContext.Products.Skip(pageSize * page + 1).Take(pageSize).ToList();
        }

        public IList<Product> GetAllBySupplier(int supplierId, int pageSize = CatalogConfiguration.DefaultRecordsPerPage, int page = 0)
        {
            return _catalogDbContext.Products.Skip(pageSize * page + 1).Take(pageSize).ToList();
        }

        public int Add(Product product)
        {
            Guard.AgainstNull(product, nameof(product));

            var result = _productValidator.Validate(product);
            if (!result.IsValid)
                throw new ValidationException(result, nameof(product));

            _catalogDbContext.Products.Add(product);
            _catalogDbContext.SaveChanges();

            return product.Id;
        }

        public int Update(Product product)
        {
            Guard.AgainstNull(product, nameof(product));

            var result = _productValidator.Validate(product);
            if (!result.IsValid)
                throw new ValidationException(result, nameof(product));

            _catalogDbContext.Products.Update(product);
            _catalogDbContext.SaveChanges();

            return product.Id;
        }

        public void Remove(Product product)
        {
            Guard.AgainstNull(product, nameof(product));

            _catalogDbContext.Products.Remove(product);
            _catalogDbContext.SaveChanges();
        }

        public void Remove(int productId)
        {
            if (productId <= 0)
                throw new ArgumentOutOfRangeException(nameof(productId), "ProductId must be greater then 0");

            var product = _catalogDbContext.Products.FirstOrDefault(p => p.Id == productId);
            if (product == null)
                throw new ArgumentException("ProductId could not be found", nameof(productId));
        }

        public bool IsSkuUnique(string sku)
        {
            return _productValidator.IsSkuUnique(sku);
        }

        public ValidationResult Validate(Product product)
        {
            return _productValidator.Validate(product);
        }
    }

    public interface IProductService
    {
        int Add(Product product);
        
        Product Get(int productId);
        Task<Product> GetAsync(int productId);
        Product Get(string sku);
        Task<Product> GetAsync(string sku);
        IList<Product> GetAll(int pageSize, int page);
        IList<Product> GetAllBySupplier(int supplierId, int pageSize, int page);

        bool IsSkuUnique(string sku);
        void Remove(int productId);
        void Remove(Product product);
        int Update(Product product);
        ValidationResult Validate(Product product);
        
    }
}
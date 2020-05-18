using BuyEngine.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
            return GetAsync(productId).Result;
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
            return GetAsync(sku).Result;
        }

        public async Task<Product> GetAsync(string sku)
        {
            return await _catalogDbContext.Products
                .AsNoTracking()
                //.Include(s => s.Supplier)
                //.Include(b => b.Brand)
                .Where(p => p.Sku.Equals(sku)).FirstOrDefaultAsync();
        }

        public IList<Product> GetAll(int pageSize, int page)
        {
            return _catalogDbContext.Products.Skip(pageSize * page + 1).Take(pageSize).ToList();
        }

        public IList<Product> GetAllBySupplier(int supplierId, int pageSize, int page)
        {
            return _catalogDbContext.Products.Skip(pageSize * page + 1).Take(pageSize).ToList();
        }

        public int Add(Product product)
        {
            return AddAsync(product).Result;
        }

        public async Task<int> AddAsync(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product), "Product can not be null");

            var result = await _productValidator.IsValidAsync(product);
            if (!result.IsValid)
                throw new ValidationException(result, nameof(product));

            await _catalogDbContext.Products.AddAsync(product);
            await _catalogDbContext.SaveChangesAsync(new CancellationToken());

            return product.Id;
        }

        public int Update(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product), "Product can not be null");

            var result = _productValidator.IsValid(product);
            if (!result.IsValid)
                throw new ArgumentException(nameof(product), "");

            _catalogDbContext.Products.Update(product);
            _catalogDbContext.SaveChanges();

            return product.Id;
        }

        public int Remove(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product), "Product can not be null");

            _catalogDbContext.Products.Remove(product);
            _catalogDbContext.SaveChanges();

            return product.Id;
        }

        public int Remove(int productId)
        {
            if (productId <= 0)
                throw new ArgumentOutOfRangeException(nameof(productId), "ProductId must be greater then 0");

            var product = _catalogDbContext.Products.FirstOrDefault(p => p.Id == productId);
            if (product == null)
                throw new ArgumentException(nameof(productId), "ProductId could not be found");

            return Remove(product);
        }

        public bool IsSkuUnique(string sku)
        {
            return _productValidator.IsSkuUnique(sku);
        }
    }

    public interface IProductService
    {
        int Add(Product product);
        Task<int> AddAsync(Product product);

        Product Get(int productId);
        Task<Product> GetAsync(int productId);
        Product Get(string sku);
        Task<Product> GetAsync(string sku);
        IList<Product> GetAll(int pageSize, int page);
        IList<Product> GetAllBySupplier(int supplierId, int pageSize, int page);

        bool IsSkuUnique(string sku);
        int Remove(int productId);
        int Remove(Product product);
        int Update(Product product);
    }
}
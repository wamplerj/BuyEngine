﻿using BuyEngine.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuyEngine.Persistence;

namespace BuyEngine.Catalog
{
    public class ProductService : IProductService
    {
        private readonly IStoreDbContext _storeDbContext;
        private readonly IProductValidator _productValidator;

        public ProductService(IStoreDbContext storeDbContext, IProductValidator productValidator)
        {
            _storeDbContext = storeDbContext;
            _productValidator = productValidator;
        }

        public Product Get(int productId)
        {
            return GetAsync(productId).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public async Task<Product> GetAsync(int productId)
        {
            var product = await _storeDbContext.Products
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
            return await _storeDbContext.Products
                .AsNoTracking()
                .Include(s => s.Supplier)
                .Include(b => b.Brand)
                .Where(p => p.Sku.Equals(sku)).FirstOrDefaultAsync();
        }

        public IList<Product> GetAll(int pageSize = CatalogConfiguration.DefaultRecordsPerPage, int page = 0)
        {
            return GetAllAsync(pageSize, page).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public async Task<IList<Product>> GetAllAsync(int pageSize = CatalogConfiguration.DefaultRecordsPerPage, int page = 0)
        {
            return await _storeDbContext.Products.Skip(page * pageSize).Take(pageSize).ToListAsync();
        }

        public IList<Product> GetAllBySupplier(int supplierId, int pageSize = CatalogConfiguration.DefaultRecordsPerPage, int page = 0)
        {
            return GetAllBySupplierAsync(supplierId, pageSize, page).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public async Task<IList<Product>> GetAllBySupplierAsync(int supplierId, int pageSize = CatalogConfiguration.DefaultRecordsPerPage, int page = 0)
        {
            return await _storeDbContext.Products.Skip(pageSize * page).Take(pageSize).ToListAsync();
        }

        public int Add(Product product)
        {
            Guard.Null(product, nameof(product));

            var result = _productValidator.Validate(product);
            if (!result.IsValid)
                throw new ValidationException(result, nameof(product));

            _storeDbContext.Products.Add(product);
            _storeDbContext.SaveChanges();

            return product.Id;
        }

        public int Update(Product product)
        {
            Guard.Null(product, nameof(product));

            var result = _productValidator.Validate(product, requireUniqueSku:false);
            if (!result.IsValid)
                throw new ValidationException(result, nameof(product));

            _storeDbContext.Products.Update(product);
            _storeDbContext.SaveChanges();

            return product.Id;
        }

        public void Remove(Product product)
        {
            Guard.Null(product, nameof(product));

            _storeDbContext.Products.Remove(product);
            _storeDbContext.SaveChanges();
        }

        public void Remove(int productId)
        {
            Guard.NegativeOrZero(productId, nameof(productId));

            var product = _storeDbContext.Products.FirstOrDefault(p => p.Id == productId);
            if (product == null)
                throw new ArgumentException("ProductId could not be found", nameof(productId));

            _storeDbContext.Products.Remove(product);
            _storeDbContext.SaveChanges();
        }

        public bool IsSkuUnique(string sku)
        {
            return _productValidator.IsSkuUnique(sku);
        }
        public Task<bool> IsSkuUniqueAsync(string sku)
        {
            return _productValidator.IsSkuUniqueAsync(sku);
        }

        public ValidationResult Validate(Product product, bool requireUniqueSku)
        {
            return _productValidator.Validate(product, requireUniqueSku);
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
        Task<IList<Product>> GetAllAsync(int pageSize, int page);
        IList<Product> GetAllBySupplier(int supplierId, int pageSize, int page);
        Task<IList<Product>> GetAllBySupplierAsync(int supplierId, int pageSize, int page);

        bool IsSkuUnique(string sku);
        Task<bool> IsSkuUniqueAsync(string sku);
        void Remove(int productId);
        void Remove(Product product);
        int Update(Product product);
        ValidationResult Validate(Product product, bool requireUniqueSku);
        
    }
}
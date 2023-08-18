using BuyEngine.Catalog;
using BuyEngine.Catalog.Brands;
using BuyEngine.Catalog.Suppliers;
using BuyEngine.Common;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BuyEngine.Data.Sql
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<ProductRepository> _logger;

        private readonly Func<Product, Brand, Supplier, Product> _mapProduct = (product, brand, supplier) =>
        {
            product.Brand = brand;
            product.Supplier = supplier;
            return product;
        };

        public ProductRepository(IConfiguration configuration, ILogger<ProductRepository> logger)
        {
            _logger = logger;
            _connectionString = configuration.GetConnectionString("BuyEngine");
        }

        public async Task<Product> GetAsync(string sku)
        {
            _logger.LogDebug("Querying for Product by Sku: {sku}", sku);
            var product = await QueryProduct(ProductQueries.GetBySku, new { sku });

            _logger.LogDebug("Product returned was {product.Id}", product.Id);
            return product;
        }

        public async Task<PagedList<Product>> GetAllAsync(int pageSize, int page)
        {
            _logger.LogDebug("Querying for Product All Products, Page: {page}, PageSize: {pageSize}", page, pageSize);

            var parameters = new { pageSize, skip = (page - 1) * pageSize };
            var products = await QueryProducts(ProductQueries.GetAll, parameters, pageSize, page);

            _logger.LogDebug("{products.Count} Products were returned", products.Count);
            return products;
        }

        public async Task<Guid> AddAsync(Product product) => throw new NotImplementedException();

        public async Task<bool> UpdateAsync(Product product)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveAsync(Product product)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExistsAsync(string sku)
        {
            throw new NotImplementedException();
        }

        public async Task<Product> GetAsync(Guid productId)
        {
            _logger.LogDebug("Querying for Product by Id: {productId}", productId);

            var product = await QueryProduct(ProductQueries.GetById, new { productid = productId });

            _logger.LogDebug($"Product returned was {product.Id}", product.Id);
            return product;
        }

        public async Task<PagedList<Product>> GetAllBySupplierAsync(Guid supplierId, int pageSize, int page)
        {
            _logger.LogDebug(
                "Querying for Product All Products for Supplier: {supplierId}, Page: {page}, PageSize: {pageSize}", supplierId, page, pageSize);

            var parameters = new { supplierId, pageSize, skip = (page - 1) * pageSize };
            var products = await QueryProducts(ProductQueries.GetAllBySupplierId, parameters, pageSize, page);

            _logger.LogDebug("{products.Count} Products were returned", products.Count);
            return products;
        }

        public async Task<PagedList<Product>> GetAllByBrandAsync(Guid brandId, int pageSize, int page)
        {
            _logger.LogDebug(
                "Querying for Product All Products for Brand: {brandId}, Page: {page}, PageSize: {pageSize}", brandId, page, pageSize);

            var parameters = new { brandId, pageSize, skip = (page - 1) * pageSize };
            var products = await QueryProducts(ProductQueries.GetAllByBrandId, parameters, pageSize, page);

            _logger.LogDebug("{products.Count} Products were returned", products.Count);
            return products;
        }

        private async Task<PagedList<Product>> QueryProducts(string sql, object parameters, int pageSize, int page)
        {
            await using var connection = new SqlConnection(_connectionString);

            sql = $"{sql}{ProductQueries.GetPageInfo}";
            _logger.LogTrace("SQL Query: {sql}", sql);

            var result = await connection.QueryMultipleAsync(sql, parameters);

            var products = result.Read(_mapProduct, "BrandId,SupplierId");
            var pageInfo = await result.ReadFirstAsync();

            pageSize = pageInfo.PageSize;
            page = pageInfo.Skip / pageSize + 1;
            int totalCount = pageInfo.TotalCount;

            return products.ToPagedList(pageSize, page, totalCount);
        }

        private async Task<Product> QueryProduct(string sql, object parameters)
        {
            await using var connection = new SqlConnection(_connectionString);

            _logger.LogTrace("SQL Query: {sql}", sql);

            var product = (await connection.QueryAsync(sql, _mapProduct, parameters, splitOn: "BrandId,SupplierId"))
                .FirstOrDefault();

            return product;
        }
    }
}
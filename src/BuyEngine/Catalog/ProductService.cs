using BuyEngine.Common;
using Microsoft.Extensions.Logging;

namespace BuyEngine.Catalog;

public class ProductService : IProductService
{
    private readonly ILogger<ProductService> _logger;
    private readonly IProductRepository _productRepository;
    private readonly IProductValidator _productValidator;

    public ProductService(IProductRepository productRepository, IProductValidator productValidator, ILogger<ProductService> logger)
    {
        _productRepository = productRepository;
        _productValidator = productValidator;
        _logger = logger;
    }

    public async Task<Product> GetAsync(Guid productId)
    {
        Guard.Default(productId, nameof(productId));

        var product = await _productRepository.GetAsync(productId);
        return product;
    }

    public async Task<Product> GetAsync(string sku)
    {
        Guard.Empty(sku, nameof(sku));

        return await _productRepository.GetAsync(sku);
    }

    public async Task<IPagedList<Product>> GetAllAsync(int pageSize = CatalogConfiguration.DefaultRecordsPerPage, int page = 0) =>
        await _productRepository.GetAllAsync(pageSize, page);

    public async Task<IPagedList<Product>> GetAllByBrandAsync(Guid brandId, int pageSize, int page)
    {
        Guard.Default(brandId, nameof(brandId));

        return await _productRepository.GetAllByBrandAsync(brandId, pageSize, page);
    }

    public async Task<IPagedList<Product>> GetAllBySupplierAsync(Guid supplierId, int pageSize = CatalogConfiguration.DefaultRecordsPerPage,
        int page = 0)
    {
        Guard.Default(supplierId, nameof(supplierId));

        return await _productRepository.GetAllBySupplierAsync(supplierId, pageSize, page);
    }

    public async Task<Guid> AddAsync(Product product)
    {
        Guard.Null(product, nameof(product));

        await _productValidator.ThrowIfInvalidAsync(product, nameof(product));

        var id = await _productRepository.AddAsync(product);
        return id;
    }

    public async Task<bool> UpdateAsync(Product product)
    {
        Guard.Null(product, nameof(product));

        await _productValidator.ThrowIfInvalidAsync(product, nameof(product));

        var success = await _productRepository.UpdateAsync(product);
        return success;
    }

    public async Task RemoveAsync(Product product)
    {
        Guard.Null(product, nameof(product));

        if (product.Quantity > 0)
        {
            _logger.LogInformation($"Product {product.Sku} has current inventory and can not be removed.  It was disabled instead.", product);

            product.Enabled = false;
            product.Sku = string.Empty;

            await UpdateAsync(product);
            return;
        }

        //TODO Ensure no orders exist for product

        await _productRepository.RemoveAsync(product);
    }

    public async Task RemoveAsync(Guid productId)
    {
        Guard.Default(productId, nameof(productId));

        var product = await _productRepository.GetAsync(productId);
        if (product == null)
            throw new ArgumentException("ProductId could not be found", nameof(productId));

        await RemoveAsync(product);
    }

    public Task<bool> IsSkuUniqueAsync(string sku)
    {
        Guard.Empty(sku, nameof(sku));

        return _productValidator.IsSkuUniqueAsync(sku);
    }

    public async Task<ValidationResult> ValidateAsync(Product product, bool requireUniqueSku = true)
    {
        Guard.Default(product, nameof(product));

        return await _productValidator.ValidateAsync(product, requireUniqueSku);
    }
}

public interface IProductService
{
    Task<Product> GetAsync(Guid productId);
    Task<Product> GetAsync(string sku);
    Task<IPagedList<Product>> GetAllAsync(int pageSize, int page);
    Task<IPagedList<Product>> GetAllByBrandAsync(Guid brandId, int pageSize, int page);
    Task<IPagedList<Product>> GetAllBySupplierAsync(Guid supplierId, int pageSize, int page);

    Task<bool> IsSkuUniqueAsync(string sku);
    Task<Guid> AddAsync(Product product);
    Task RemoveAsync(Guid productId);
    Task RemoveAsync(Product product);
    Task<bool> UpdateAsync(Product product);
    Task<ValidationResult> ValidateAsync(Product product, bool requireUniqueSku = true);
}
using BuyEngine.Common;

namespace BuyEngine.Catalog;

public class ProductValidator : IProductValidator
{
    private readonly IProductRepository _productRepository;

    public ProductValidator(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ValidationResult> ValidateAsync(Product product) => await ValidateAsync(product, true);

    public async Task<ValidationResult> ValidateAsync(Product product, bool isProductNew)
    {
        var result = new ValidationResult();

        if (string.IsNullOrWhiteSpace(product.Sku) && product.Enabled)
            result.AddMessage(nameof(product.Sku), "Product SKU is Required");

        if (string.IsNullOrWhiteSpace(product.Name))
            result.AddMessage(nameof(product.Name), "Product Name is Required");

        if (product.Price < decimal.Zero)
            result.AddMessage(nameof(product.Price), $"Product {nameof(product.Price)} must be greater then or equal to zero");

        if (!isProductNew || !product.Enabled) return result;

        var unique = await IsSkuUniqueAsync(product.Sku);
        if (!unique)
            result.AddMessage("SkuUnique", "Product SKU must be Unique");

        return result;
    }

    public async Task<bool> IsSkuUniqueAsync(string sku)
    {
        if (string.IsNullOrWhiteSpace(sku)) return false;

        var exists = await _productRepository.ExistsAsync(sku);
        return !exists;
    }
}

public interface IProductValidator : IModelValidator<Product>
{
    Task<ValidationResult> ValidateAsync(Product product, bool isProductNew);
    Task<bool> IsSkuUniqueAsync(string sku);
}
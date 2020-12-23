using BuyEngine.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BuyEngine.Catalog
{
    public class ProductValidator: IProductValidator
    {
        private readonly ICatalogDbContext _catalogDbContext;

        public ProductValidator(ICatalogDbContext catalogDbContext)
        {
            _catalogDbContext = catalogDbContext;
        }

        public ValidationResult Validate(Product product)
        {
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(product.Sku))
                result.AddMessage(nameof(product.Sku), "Product SKU is Required");

            var unique = IsSkuUnique(product.Sku);
            if (!unique)
                result.AddMessage(nameof(product.Sku), "Product SKU must be Unique");

            if (string.IsNullOrWhiteSpace(product.Name))
                result.AddMessage(nameof(product.Name), "Product Name is Required");
            
            if(product.Price < decimal.Zero)
                result.AddMessage(nameof(product.Price), $"Product {nameof(product.Price)} must be greater then or equal to zero");

            return result;
        }

        public bool IsSkuUnique(string sku)
        {
            return IsSkuUniqueAsync(sku).GetAwaiter().GetResult();
        }

        public async Task<bool> IsSkuUniqueAsync(string sku)
        {
            return await _catalogDbContext.Products.AnyAsync(p => p.Sku.Equals(sku, StringComparison.OrdinalIgnoreCase)).ContinueWith(n => !n.Result);
        }
    }

    public interface IProductValidator : IModelValidator<Product>
    {
        bool IsSkuUnique(string sku);
        Task<bool> IsSkuUniqueAsync(string sku);
    }
}

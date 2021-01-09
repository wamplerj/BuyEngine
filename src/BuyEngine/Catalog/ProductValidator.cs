using BuyEngine.Common;
using Microsoft.EntityFrameworkCore;
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
            return Validate(product, requireUniqueSku: true);
        }

        public ValidationResult Validate(Product product, bool requireUniqueSku)
        {
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(product.Sku))
                result.AddMessage(nameof(product.Sku), "Product SKU is Required");

            if (string.IsNullOrWhiteSpace(product.Name))
                result.AddMessage(nameof(product.Name), "Product Name is Required");
            
            if(product.Price < decimal.Zero)
                result.AddMessage(nameof(product.Price), $"Product {nameof(product.Price)} must be greater then or equal to zero");

            if (!requireUniqueSku) return result;
            
            var unique = IsSkuUnique(product.Sku);
            if (!unique)
                result.AddMessage(nameof(product.Sku), "Product SKU must be Unique");

            return result;
        }

        public bool IsSkuUnique(string sku)
        {
            return IsSkuUniqueAsync(sku).GetAwaiter().GetResult();
        }

        public async Task<bool> IsSkuUniqueAsync(string sku)
        {
            return await _catalogDbContext.Products.AnyAsync(p => p.Sku.Equals(sku)).ContinueWith(n => !n.Result);
        }
    }

    public interface IProductValidator : IModelValidator<Product>
    {
        ValidationResult Validate(Product product, bool requireUniqueSku);
        bool IsSkuUnique(string sku);
        Task<bool> IsSkuUniqueAsync(string sku);
    }
}

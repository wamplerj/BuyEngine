using BuyEngine.Common;
using System;
using System.Linq;

namespace BuyEngine.Catalog
{
    public class ProductValidator: IProductValidator
    {
        private readonly ICatalogDbContext _catalogDbContext;

        public ProductValidator(ICatalogDbContext catalogDbContext)
        {
            _catalogDbContext = catalogDbContext;
        }

        public ValidationResult IsValid(Product product)
        {
            var result = new ValidationResult();

            if(string.IsNullOrWhiteSpace(product.Sku))
                result.AddMessage(nameof(product.Sku), "Product SKU is Required");

            if(!IsSkuUnique(product.Sku))
                result.AddMessage(nameof(product.Sku), "Product SKU must be Unique");

            if(string.IsNullOrWhiteSpace(product.Name))
                result.AddMessage(nameof(product.Name), "Product Name is Required");

            return result;
        }

        public bool IsSkuUnique(string sku)
        {
            return !_catalogDbContext.Products.Any(p => p.Sku.Equals(sku, StringComparison.OrdinalIgnoreCase));
        }
    }

    public interface IProductValidator : IModelValidator<Product>
    {
        bool IsSkuUnique(string sku);
    }
}

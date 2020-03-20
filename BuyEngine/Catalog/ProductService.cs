using BuyEngine.Common;
using System;
using System.Linq;

namespace BuyEngine.Catalog
{
    public class ProductService
    {
        private readonly ICatalogDbContext _catalogDbContext;
        private readonly IProductValidator _productValidator;

        public ProductService(ICatalogDbContext catalogDbContext, IProductValidator productValidator)
        {
            _catalogDbContext = catalogDbContext;
            _productValidator = productValidator;
        }

        public int Add(Product product)
        {
            if(product == null)
                throw new ArgumentNullException(nameof(product), "Product can not be null");

            var result = _productValidator.IsValid(product);
            if(!result.IsValid)
                throw new ArgumentException(nameof(product), "");

            _catalogDbContext.Products.Add(product);
            _catalogDbContext.SaveChanges();

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
            if(product == null)
                throw new ArgumentException(nameof(productId), "ProductId could not be found");

            return Remove(product);
        }

        public bool IsSkuUnique(string sku)
        {
            return _productValidator.IsSkuUnique(sku);
        }

    }
}

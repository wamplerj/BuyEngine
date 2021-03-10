using System.Threading.Tasks;
using BuyEngine.Common;

namespace BuyEngine.Catalog.Brands
{
    public class BrandValidator : IModelValidator<Brand>
    {
        public async Task<ValidationResult> ValidateAsync(Brand brand)
        {
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(brand.Name))
                result.AddMessage(nameof(brand.Name), $"{nameof(brand.Name)} is Required");

            return result;
        }
    }
}
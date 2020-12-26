using BuyEngine.Common;

namespace BuyEngine.Catalog.Brands
{
    public class BrandValidator : IModelValidator<Brand>
    {
        public ValidationResult Validate(Brand brand)
        {
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(brand.Name))
                result.AddMessage(nameof(brand.Name), $"{nameof(brand.Name)} is Required");

            return result;
        }
    }
}

using BuyEngine.Common;

namespace BuyEngine.Catalog.Suppliers
{
    public class SupplierValidator : IModelValidator<Supplier>
    {
        public ValidationResult Validate(Supplier brand)
        {
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(brand.Name))
                result.AddMessage(nameof(brand.Name), "Supplier Name is Required");

            return result;
        }
    }
}

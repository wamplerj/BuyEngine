using BuyEngine.Common;

namespace BuyEngine.Catalog.Suppliers
{
    public class SupplierValidator : IModelValidator<Supplier>
    {
        public ValidationResult Validate(Supplier supplier)
        {
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(supplier.Name))
                result.AddMessage(nameof(supplier.Name), "Supplier Name is Required");

            return result;
        }
    }
}

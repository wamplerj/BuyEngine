using System.Threading.Tasks;
using BuyEngine.Common;

namespace BuyEngine.Catalog.Suppliers
{
    public class SupplierValidator : IModelValidator<Supplier>
    {
        public async Task<ValidationResult> ValidateAsync(Supplier brand)
        {
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(brand.Name))
                result.AddMessage(nameof(brand.Name), "Supplier Name is Required");

            return result;
        }
    }
}

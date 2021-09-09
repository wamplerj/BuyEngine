using System.Threading.Tasks;
using BuyEngine.Common;

namespace BuyEngine.Customer
{
    public class AddressValidator : IModelValidator<Address>
    {
        public async Task<ValidationResult> ValidateAsync(Address model)
        {
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(model.Line1))
                result.AddMessage(nameof(model.Line1), "Address Line1 is required");

            if (string.IsNullOrWhiteSpace(model.City))
                result.AddMessage(nameof(model.City), "City is required");

            if (string.IsNullOrWhiteSpace(model.StateProvince))
                result.AddMessage(nameof(model.StateProvince), "State/Province is required");

            if (string.IsNullOrWhiteSpace(model.PostalCode))
                result.AddMessage(nameof(model.PostalCode), "Postal Code is required");

            if (string.IsNullOrWhiteSpace(model.CountryRegion))
                result.AddMessage(nameof(model.CountryRegion), "Country or Region is required");

            return result;
        }
    }
}

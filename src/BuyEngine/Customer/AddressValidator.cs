using BuyEngine.Common;
using System.Threading.Tasks;

namespace BuyEngine.Customer
{
    public class AddressValidator : IModelValidator<Address>
    {
        public async Task<ValidationResult> ValidateAsync(Address address)
        {
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(address.Line1))
                result.AddMessage(nameof(address.Line1), "Address Line1 is required");

            if (string.IsNullOrWhiteSpace(address.City))
                result.AddMessage(nameof(address.City), "City is required");

            if (string.IsNullOrWhiteSpace(address.StateProvince))
                result.AddMessage(nameof(address.StateProvince), "State/Province is required");

            if (string.IsNullOrWhiteSpace(address.PostalCode))
                result.AddMessage(nameof(address.PostalCode), "Postal Code is required");

            if (string.IsNullOrWhiteSpace(address.CountryRegion))
                result.AddMessage(nameof(address.CountryRegion), "Country or Region is required");

            return result;
        }
    }
}

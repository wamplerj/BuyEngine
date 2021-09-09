using System.Threading.Tasks;
using BuyEngine.Common;
using BuyEngine.Customer;

namespace BuyEngine.Checkout
{
    public class SalesOrderAddressValidator : IModelValidator<SalesOrderAddress>
    {
        private readonly IModelValidator<Address> _addressValidator;

        public SalesOrderAddressValidator(IModelValidator<Address> addressValidator)
        {
            _addressValidator = addressValidator;
        }

        public async Task<ValidationResult> ValidateAsync(SalesOrderAddress address)
        {
            var result = await _addressValidator.ValidateAsync(address);

            if (string.IsNullOrWhiteSpace(address.Customer))
                result.AddMessage(nameof(address.Customer), "Customer Name is required");

            if (string.IsNullOrWhiteSpace(address.EmailAddress))
                result.AddMessage(nameof(address.EmailAddress), "Email Address is required");

            if (string.IsNullOrWhiteSpace(address.PhoneNumber))
                result.AddMessage(nameof(address.PhoneNumber), "Phone Number is required");

            return result;
        }
    }
}

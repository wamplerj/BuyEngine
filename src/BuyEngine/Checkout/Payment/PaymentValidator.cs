using System.Threading.Tasks;
using BuyEngine.Common;

namespace BuyEngine.Checkout.Payment
{
    public class PaymentValidator : IModelValidator<PaymentInformation>
    {
        public async Task<ValidationResult> ValidateAsync(PaymentInformation payment)
        {
            //TODO Validate Payment Info

            var result = new ValidationResult();
            return result;
        }
    }
}

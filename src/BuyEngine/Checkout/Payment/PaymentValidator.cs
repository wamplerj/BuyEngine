using BuyEngine.Common;
using System;
using System.Threading.Tasks;

namespace BuyEngine.Checkout.Payment
{
    public class PaymentValidator : IModelValidator<PaymentInformation>
    {
        public async Task<ValidationResult> ValidateAsync(PaymentInformation payment)
        {
            var result = new ValidationResult();

            if (payment.IsAuthorized) return result;

            if (string.IsNullOrWhiteSpace(payment.CreditCardNumber))
                result.AddMessage(nameof(payment.CreditCardNumber), "Credit Card Number is required");

            if (string.IsNullOrWhiteSpace(payment.Ccv))
                result.AddMessage(nameof(payment.Ccv), "Credit Card Verification Number (CCV) is required");

            if (string.IsNullOrWhiteSpace(payment.Payee))
                result.AddMessage(nameof(payment.Payee), "Name on Credit Card is required");

            if (payment.Expiration <= DateTime.Today)
                result.AddMessage(nameof(payment.Expiration), "Expiration Date is required");

            if (string.IsNullOrWhiteSpace(payment.PostalCode))
                result.AddMessage(nameof(payment.PostalCode), "Postal Code is required");

            return result;
        }
    }
}

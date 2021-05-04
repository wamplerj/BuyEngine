using System;
using BuyEngine.Common;
using System.Threading.Tasks;

namespace BuyEngine.Checkout
{
    public class CartValidator : IModelValidator<Cart>
    {
        public async Task<ValidationResult> ValidateAsync(Cart cart)
        {
            var result = new ValidationResult();

            if(cart.Expires <= DateTime.UtcNow)
                result.AddMessage(nameof(cart.Expires), "Cart has expired");

            return result;
        }
    }
}

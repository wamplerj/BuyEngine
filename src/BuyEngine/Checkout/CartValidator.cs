using BuyEngine.Common;

namespace BuyEngine.Checkout;

public class CartValidator : IModelValidator<Cart>
{
    public async Task<ValidationResult> ValidateAsync(Cart cart)
    {
        var result = new ValidationResult();

        if (cart.Expires <= DateTime.UtcNow)
            result.AddMessage(nameof(cart.Expires), "Cart has expired");

        if (cart.Items.Count == 0)
            result.AddMessage(nameof(cart.Items), "Cart contains no items");

        return result;
    }
}
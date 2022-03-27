using BuyEngine.Common;

namespace BuyEngine.Checkout.Shipping;

public class ShippingValidator : IModelValidator<ShippingMethod>
{
    public async Task<ValidationResult> ValidateAsync(ShippingMethod shippingMethod)
    {
        var result = new ValidationResult();

        if (shippingMethod == null)
        {
            result.AddMessage(nameof(shippingMethod), "A shipping method is required");
            return result;
        }

        if (string.IsNullOrWhiteSpace(shippingMethod.Name))
            result.AddMessage(nameof(shippingMethod.Name), "Shipping Method must have a name");

        if (string.IsNullOrWhiteSpace(shippingMethod.Timeframe))
            result.AddMessage(nameof(shippingMethod.Timeframe), "Shipping Method must have a timeframe");

        if (shippingMethod.Price < decimal.Zero)
            result.AddMessage(nameof(shippingMethod.Price), "Shipping Method must have Price");

        return result;
    }
}
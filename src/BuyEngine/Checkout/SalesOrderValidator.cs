using BuyEngine.Common;

namespace BuyEngine.Checkout;

public class SalesOrderValidator : IModelValidator<SalesOrder>
{
    public Task<ValidationResult> ValidateAsync(SalesOrder model) => throw new NotImplementedException();
}
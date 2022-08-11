namespace BuyEngine.Checkout;

public class Order
{
}

public record OrderResult
{
    public bool Success => OrderId.HasValue && string.IsNullOrWhiteSpace(Message);
    public string Message { get; set; }
    public Guid? OrderId { get; set; }

    public override string ToString() => $"Order Result: {Success}, OrderId: {OrderId}, Message: {Message}";
}
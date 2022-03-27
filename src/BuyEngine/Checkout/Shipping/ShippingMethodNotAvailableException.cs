namespace BuyEngine.Checkout.Shipping;

public class ShippingMethodNotAvailableException : Exception
{
    private readonly string _message;

    public ShippingMethodNotAvailableException(string message)
    {
        _message = message;
    }
}
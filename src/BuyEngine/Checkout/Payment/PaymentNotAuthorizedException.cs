namespace BuyEngine.Checkout.Payment;

public class PaymentNotAuthorizedException : Exception
{
    private readonly string _message;

    public PaymentNotAuthorizedException(string message)
    {
        _message = message;
    }
}
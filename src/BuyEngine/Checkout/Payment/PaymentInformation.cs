namespace BuyEngine.Checkout.Payment;

public class PaymentInformation
{
    public string Payee { get; set; }
    public string CreditCardNumber { get; set; }
    public string Ccv { get; set; }
    public DateTime Expiration { get; set; }
    public string PostalCode { get; set; }
    public string AuthorizationCode { get; set; }

    public bool IsAuthorized => !string.IsNullOrWhiteSpace(AuthorizationCode);
}
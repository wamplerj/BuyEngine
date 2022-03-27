namespace BuyEngine.Checkout.Payment;

public class PaymentService : IPaymentService
{
    public async Task<bool> IsAuthValidAsync(PaymentInformation paymentInformation) =>
        //TODO Verify AuthCode with Payment Provider
        paymentInformation.IsAuthorized;

    public async Task<Guid> CollectPaymentAsync(PaymentInformation paymentInformation) => Guid.NewGuid();
}

public interface IPaymentService
{
    Task<bool> IsAuthValidAsync(PaymentInformation paymentInformation);
    Task<Guid> CollectPaymentAsync(PaymentInformation paymentInformation);
}
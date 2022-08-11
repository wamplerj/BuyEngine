namespace BuyEngine.Checkout;

public class OrderService : IOrderService
{
    public async Task<OrderResult> AddAsync(Order order) => throw new NotImplementedException();
}

public interface IOrderService
{
    Task<OrderResult> AddAsync(Order order);
}
namespace BuyEngine.Checkout;

public class OrderService : IOrderService
{
    public async Task<Guid> AddAsync(Order order) => throw new NotImplementedException();
}

public interface IOrderService
{
    Task<Guid> AddAsync(Order order);
}
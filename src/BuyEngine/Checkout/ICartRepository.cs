using System;
using System.Threading.Tasks;

namespace BuyEngine.Checkout
{
    public interface ICartRepository
    {
        Task<Cart> GetAsync(Guid cartId);
        Task<Guid> Add(Cart cart);
        Task<bool> Update(Cart cart);
        Task<bool> Delete(Guid cartId);
    }
}
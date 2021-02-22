using BuyEngine.Common;
using System;
using System.Threading.Tasks;

namespace BuyEngine.Checkout
{
    public class CartService : ICartService
    {
        public CartService()
        {
            
        }
        
        public Cart Get(Guid cartId)
        {
            return new Cart();
        }

        public async Task<Cart> GetAsync(Guid cartId)
        {
            Guard.Default(cartId, nameof(cartId));

            return null;
        }
    }

    public interface ICartService
    {
        Cart Get(Guid cartId);
        Task<Cart> GetAsync(Guid cartId);
    }
}

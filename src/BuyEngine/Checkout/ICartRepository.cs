using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyEngine.Checkout
{
    public class InMemoryCartRepository : ICartRepository
    {
        private readonly List<Cart> _carts = new();

        public async Task<Cart> GetAsync(Guid cartId)
        {
            return _carts.Single(c => c.Id == cartId);
        }

        public async Task<Guid> Add(Cart cart)
        {
            if (cart.Id != Guid.Empty || _carts.Any(c => c.Id == cart.Id))
            {
                var _ = await Update(cart);
                return cart.Id;
            }

            cart.Id = Guid.NewGuid();
            _carts.Add(cart);
            return cart.Id;
        }

        public async Task<bool> Update(Cart cart)
        {
            if (_carts.Any(c => c.Id != cart.Id))
                return await Add(cart) != Guid.Empty;

            var old = _carts.Single(c => c.Id == cart.Id);
            _carts.Remove(old);

            _carts.Add(cart);
            return true;
        }

        public async Task<bool> Delete(Guid cartId)
        {
            if (cartId == Guid.Empty) return true;

            var cart = _carts.Single(c => c.Id == cartId);
            _carts.Remove(cart);

            return true;
        }
    }

    public interface ICartRepository
    {
        Task<Cart> GetAsync(Guid cartId);
        Task<Guid> Add(Cart cart);
        Task<bool> Update(Cart cart);
        Task<bool> Delete(Guid cartId);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace BuyEngine.Checkout
{
    public class Cart
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Expires { get; set; }

        public List<CartItem> Items { get; set; } = new();

        public decimal Total => Items.Sum(i => i.Total);

        public bool IsExpired => Expires <= DateTime.UtcNow;
    }
}
using System;
using System.Collections.Generic;

namespace BuyEngine.Checkout
{
    public class Cart
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Expires { get; set; }

        public List<CartItem> Items { get; set; } = new();
    }
}
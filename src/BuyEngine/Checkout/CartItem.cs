﻿using System;

namespace BuyEngine.Checkout
{
    public class CartItem
    {
        public int Id { get; set; }
        public Guid ProductId { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public decimal Total => Quantity * Price;
    }
}
using System;

namespace BuyEngine.Checkout
{
    public class CartItem
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal ItemWeight { get; set; }

        public int Quantity { get; set; }

        public decimal Total => Quantity * Price;

        public bool IsNew => Id == default;
    }
}
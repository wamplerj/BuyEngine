using BuyEngine.Catalog;

namespace BuyEngine.Checkout
{
    public class CartItem
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }

        public decimal Total => Quantity * Product.Price;
    }
}
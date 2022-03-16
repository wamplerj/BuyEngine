using BuyEngine.Catalog.Brands;
using BuyEngine.Catalog.Suppliers;

namespace BuyEngine.Catalog
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public bool Enabled { get; set; } = true;

        public Brand Brand { get; set; }

        public Supplier Supplier { get; set; }

        public override string ToString() => $"Product: {Id} - {Sku}";
    }
}
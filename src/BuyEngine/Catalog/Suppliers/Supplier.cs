using System.Collections.Generic;

namespace BuyEngine.Catalog.Suppliers
{
    public class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string WebsiteUrl { get; set; }
        public string Notes { get; set; }
        public List<Product> Products { get; set; }
    }
}

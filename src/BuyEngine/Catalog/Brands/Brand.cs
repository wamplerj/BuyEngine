using System;
using System.Collections.Generic;

namespace BuyEngine.Catalog.Brands
{
    public class Brand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string WebsiteUrl { get; set; }
        public string Notes { get; set; }
        public List<Product> Products { get; set; }
    }
}

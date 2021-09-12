using System;
using System.Collections.Generic;

namespace BuyEngine.Customer
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string, Address> Addresses { get; set; } = new();
    }
}

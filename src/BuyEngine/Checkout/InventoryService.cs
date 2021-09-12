using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyEngine.Checkout
{
    public class InventoryService : IInventoryService
    {
        public Task<InventoryStatus> IsAvailable(IEnumerable<string> skus)
        {
            throw new NotImplementedException();
        }
    }

    public interface IInventoryService
    {
        Task<InventoryStatus> IsAvailable(IEnumerable<string> skus);
    }

    public class InventoryStatus
    {
        private readonly ICollection<ProductInventory> _products;

        public InventoryStatus(ICollection<ProductInventory> products)
        {
            _products = products;
        }


        public void ThrowIfOutOfStock()
        {
            if (_products.Any(p => p.OutOfStock))
                throw new ProductOutOfStockException($"Out of Stock");
        }
    }

    public class ProductOutOfStockException : Exception
    {
        private readonly string _message;

        public ProductOutOfStockException(string message)
        {
            _message = message;
        }
    }

    public class ProductInventory
    {
        public bool OutOfStock { get; set; }
    }



}

using System.Collections.Generic;

namespace BuyEngine.Catalog.Suppliers
{
    public class SupplierService
    {
        private readonly ICatalogDbContext _dbContext;

        public SupplierService(ICatalogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int Get(int supplierId)
        {
            return 0;
        }

        public IList<Supplier> GetAll()
        {
            return null;
        }

        public IList<Supplier> GetAll(int pageSize = 25, int page = 0)
        {

            return null;
        }

        public int Add(Supplier supplier)
        {
            return 0;
        }

        public void Update(Supplier supplier)
        {

        }

        public void Delete(Supplier supplier)
        {

        }

        public void Delete(int supplierId)
        {

        }

    }
}

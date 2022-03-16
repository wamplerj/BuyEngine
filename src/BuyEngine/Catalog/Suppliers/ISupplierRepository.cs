namespace BuyEngine.Catalog.Suppliers
{
    internal class NullSupplierRepository : ISupplierRepository
    {
        public Task<Supplier> GetAsync(Guid brandId) => throw new NotImplementedException();

        public Task<IList<Supplier>> GetAllAsync(int pageSize, int page) => throw new NotImplementedException();

        public Task<Guid> AddAsync(Supplier brand) => throw new NotImplementedException();

        public Task<bool> UpdateAsync(Supplier brand) => throw new NotImplementedException();

        public Task RemoveAsync(Supplier brand) => throw new NotImplementedException();
    }

    public interface ISupplierRepository
    {
        Task<Supplier> GetAsync(Guid brandId);
        Task<IList<Supplier>> GetAllAsync(int pageSize, int page);
        Task<Guid> AddAsync(Supplier brand);
        Task<bool> UpdateAsync(Supplier brand);
        Task RemoveAsync(Supplier brand);
    }
}
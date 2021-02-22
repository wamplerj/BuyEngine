using System.Threading;
using System.Threading.Tasks;
using BuyEngine.Catalog;
using BuyEngine.Catalog.Brands;
using BuyEngine.Catalog.Suppliers;
using BuyEngine.Checkout;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BuyEngine.Persistence
{
    public class StoreDbContext : DbContext, IStoreDbContext
    {

        public DbSet<Product> Products { get; set; }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<Cart> Carts { get; set; }

        public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //TODO: Load DbSchema via CatalogConfiguration 
            modelBuilder.HasDefaultSchema("BuyEngine");

            modelBuilder.ApplyConfiguration(new BrandTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProductTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SupplierTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CartTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CartItemTypeConfiguration());
        }
    }

    public interface IStoreDbContext
    {
        DbSet<Product> Products { get; set; }
        DbSet<Brand> Brands { get; set; }
        DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Cart> Carts { get; set; }

        DatabaseFacade Database { get; }
        EntityEntry Entry(object entity);
        int SaveChanges();
        int SaveChanges(bool acceptAllChangesOnSuccess);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken);
    }
}
using BuyEngine.Catalog.Suppliers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BuyEngine.Catalog
{
    public class CatalogDbContext : DbContext, ICatalogDbContext
    {

        public DbSet<Product> Products { get; set; }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //TODO: Load DbSchema via CatalogConfiguration 
            modelBuilder.HasDefaultSchema("BuyEngine");

            modelBuilder.ApplyConfiguration(new BrandTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProductTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SupplierTypeConfiguration());
        }
    }

    public interface ICatalogDbContext
    {
        DbSet<Product> Products { get; set; }
        DbSet<Brand> Brands { get; set; }
        DbSet<Supplier> Suppliers { get; set; }
        

        DatabaseFacade Database { get; }
        EntityEntry Entry(object entity);
        int SaveChanges();
        int SaveChanges(bool acceptAllChangesOnSuccess);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken);
    }
}
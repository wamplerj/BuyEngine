using BuyEngine.Catalog.Brands;
using BuyEngine.Catalog.Suppliers;
using BuyEngine.Common;
using Microsoft.Extensions.DependencyInjection;

namespace BuyEngine.Catalog
{
    public static class CatalogRegistrations
    {
        public static IServiceCollection AddCatalogServices(this IServiceCollection services)
        {
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IProductValidator, ProductValidator>();
            services.AddTransient<ISupplierService, SupplierService>();
            services.AddTransient<IModelValidator<Supplier>, SupplierValidator>();
            services.AddTransient<IBrandService, BrandService>();
            services.AddTransient<IModelValidator<Brand>, BrandValidator>();

            return services;
        }
    }
}

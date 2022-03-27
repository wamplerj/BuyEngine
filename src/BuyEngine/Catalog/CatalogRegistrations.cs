using BuyEngine.Catalog.Brands;
using BuyEngine.Catalog.Suppliers;
using BuyEngine.Common;
using Microsoft.Extensions.DependencyInjection;

namespace BuyEngine.Catalog;

public static class CatalogRegistrations
{
    public static IServiceCollection AddCatalogServices(this IServiceCollection services, bool enableInMemoryRepositories = false)
    {
        services.AddTransient<IProductService, ProductService>();
        services.AddTransient<IProductValidator, ProductValidator>();
        services.AddTransient<ISupplierService, SupplierService>();
        services.AddTransient<IModelValidator<Supplier>, SupplierValidator>();
        services.AddTransient<IBrandService, BrandService>();
        services.AddTransient<IModelValidator<Brand>, BrandValidator>();

        if (!enableInMemoryRepositories) return services;

        services.AddSingleton<ICatalogDataStore, InMemoryDataStore>();
        services.AddSingleton<IProductRepository, InMemoryProductRepository>();
        services.AddTransient<IBrandRepository, InMemoryBrandRepository>();
        services.AddTransient<ISupplierRepository, InMemorySupplierRepository>();

        return services;
    }
}
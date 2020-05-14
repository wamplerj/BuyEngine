using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BuyEngine.Catalog
{
    public static class CatalogRegistrations
    {

        public static IServiceCollection AddCatalogServices(this IServiceCollection services)
        {
            services.AddDbContext<CatalogDbContext>(opt => opt.UseInMemoryDatabase("BE-Dev"));
            services.AddScoped<ICatalogDbContext, CatalogDbContext>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IProductValidator, ProductValidator>();

            return services;
        }


    }
}

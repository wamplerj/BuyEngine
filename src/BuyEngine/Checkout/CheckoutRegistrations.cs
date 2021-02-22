using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BuyEngine.Checkout
{
    public static class CheckoutRegistrations
    {

        public static IServiceCollection AddCheckoutServices(this IServiceCollection services)
        {
            services.AddTransient<ICartService, CartService>();
            //services.AddTransient<IProductValidator, ProductValidator>();
            //services.AddTransient<ISupplierService, SupplierService>();
            //services.AddTransient<IModelValidator<Supplier>, SupplierValidator>();
            //services.AddTransient<IBrandService, BrandService>();
            //services.AddTransient<IModelValidator<Brand>, BrandValidator>();

            return services;
        }
    }
}

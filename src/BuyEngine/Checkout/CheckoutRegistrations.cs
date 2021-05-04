using BuyEngine.Common;
using Microsoft.Extensions.DependencyInjection;

namespace BuyEngine.Checkout
{
    public static class CheckoutRegistrations
    {

        public static IServiceCollection AddCheckoutServices(this IServiceCollection services)
        {
            services.AddTransient<ICartService, CartService>();
            services.AddTransient<IModelValidator<Cart>, CartValidator>();

            return services;
        }
    }
}

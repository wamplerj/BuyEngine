using Microsoft.Extensions.DependencyInjection;

namespace BuyEngine.Checkout
{
    public static class CheckoutRegistrations
    {

        public static IServiceCollection AddCheckoutServices(this IServiceCollection services)
        {
            services.AddTransient<ICartService, CartService>();
            return services;
        }
    }
}

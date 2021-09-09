using BuyEngine.Checkout.Payment;
using BuyEngine.Checkout.Shipping;
using BuyEngine.Common;
using Microsoft.Extensions.DependencyInjection;

namespace BuyEngine.Checkout
{
    public static class CheckoutRegistrations
    {

        public static IServiceCollection AddCheckoutServices(this IServiceCollection services)
        {
            services.AddTransient<ICartService, CartService>();
            services.AddTransient<IShippingService, ShippingService>();
            services.AddTransient<IModelValidator<Cart>, CartValidator>();
            services.AddTransient<IModelValidator<ShippingMethod>, ShippingValidator>();
            services.AddTransient<IPaymentService, PaymentService>();

            return services;
        }
    }
}

using BuyEngine.Catalog;
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
            services.AddSingleton<ICartRepository, InMemoryCartRepository>();
            services.AddSingleton<IProductRepository, InMemoryProductRepository>();
            services.AddTransient<ICheckoutService, CheckoutService>();
            services.AddTransient<IShippingService, ShippingService>();
            services.AddTransient<IModelValidator<Cart>, CartValidator>();
            services.AddTransient<IModelValidator<ShippingMethod>, ShippingValidator>();
            services.AddTransient<IModelValidator<SalesOrder>, SalesOrderValidator>();
            services.AddTransient<IPaymentService, PaymentService>();
            services.AddTransient<IInventoryService, InventoryService>();
            services.AddTransient<IOrderService, OrderService>();

            return services;
        }
    }
}

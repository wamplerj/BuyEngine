using System;
using System.Linq;
using System.Threading.Tasks;
using BuyEngine.Checkout.Payment;
using BuyEngine.Checkout.Shipping;
using BuyEngine.Common;
using Microsoft.Extensions.Logging;

namespace BuyEngine.Checkout
{
    public class CheckoutService
    {
        private readonly IModelValidator<SalesOrder> _validator;
        private readonly IInventoryService _inventoryService;
        private readonly IShippingService _shippingService;
        private readonly IPaymentService _paymentService;
        private readonly IOrderService _orderService;
        private readonly ILogger<CheckoutService> _logger;

        public CheckoutService(IModelValidator<SalesOrder> validator, IInventoryService inventoryService, IShippingService shippingService,
            IPaymentService paymentService, IOrderService orderService, ILogger<CheckoutService> logger)
        {
            _validator = validator;
            _inventoryService = inventoryService;
            _shippingService = shippingService;
            _paymentService = paymentService;
            _orderService = orderService;
            _logger = logger;
        }
        
        public async Task<Guid> CheckoutAsync(SalesOrder salesOrder)
        {
            await VerifyInventoryAvailable(salesOrder);
            await VerifyShippingMethod(salesOrder);
            await ProcessPayment(salesOrder);

            var orderId = await ProcessOrder(salesOrder);
            return orderId;
        }

        private async Task VerifyShippingMethod(SalesOrder salesOrder)
        {
            var canShip = await _shippingService.IsShippingAvailable(salesOrder.ShippingMethod, salesOrder.BillTo);
            if (!canShip)
                throw new ShippingMethodNotAvailableException(
                    $"ShippingMethod: {salesOrder.ShippingMethod.Name} is not available for BillingAddress: {salesOrder.BillTo.ToString()}");
        }

        private async Task<Guid> ProcessOrder(SalesOrder salesOrder)
        {
            var order = salesOrder.ToOrder();
            var orderId = await _orderService.AddAsync(order);
            return orderId;
        }

        private async Task ProcessPayment(SalesOrder salesOrder)
        {
            var authValid = await _paymentService.IsAuthValidAsync(salesOrder.Payment);
            if (!authValid)
                throw new PaymentNotAuthorizedException(
                    $"Payment for SalesOrder: {salesOrder.Id} has not been authorized.  Payment can not be processed");

            var transactionId = await _paymentService.CollectPaymentAsync(salesOrder.Payment);
            _logger.LogInformation($"Payment processed for Sales Order: ");
        }

        private async Task VerifyInventoryAvailable(SalesOrder salesOrder)
        {
            //TODO Implement Backorder functionality if enabled
            var skus = salesOrder.Cart.Items.Select(i => i.Sku);

            var skuInventory = await _inventoryService.IsAvailable(skus);
            skuInventory.ThrowIfOutOfStock(); 
        }
    }
}

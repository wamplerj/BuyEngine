using BuyEngine.Checkout.Payment;
using BuyEngine.Checkout.Shipping;
using BuyEngine.Common;
using Microsoft.Extensions.Logging;

namespace BuyEngine.Checkout;

public class CheckoutService : ICheckoutService
{
    private readonly IInventoryService _inventoryService;
    private readonly ILogger<CheckoutService> _logger;
    private readonly IOrderService _orderService;
    private readonly IPaymentService _paymentService;
    private readonly IShippingService _shippingService;
    private readonly IModelValidator<SalesOrder> _validator;

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

    public async Task<OrderResult> CheckoutAsync(SalesOrder salesOrder)
    {
        var itemsAvailable = await VerifyInventoryAvailable(salesOrder);
        if (!itemsAvailable)
            return new OrderResult
                { Message = $"Inventory Availability: {salesOrder.ShippingMethod.Name} is not available for Shipping Address: {salesOrder.ShipTo}" };


        var canShip = await VerifyShippingMethod(salesOrder);
        if (!canShip)
            return new OrderResult
                { Message = $"ShippingMethod: {salesOrder.ShippingMethod.Name} is not available for Shipping Address: {salesOrder.ShipTo}" };

        await ProcessPayment(salesOrder);

        var result = await ProcessOrder(salesOrder);
        return result;
    }

    private async Task<bool> VerifyShippingMethod(SalesOrder salesOrder)
    {
        var canShip = await _shippingService.IsShippingAvailable(salesOrder.ShippingMethod, salesOrder.ShipTo);
        return canShip;
    }

    private async Task<OrderResult> ProcessOrder(SalesOrder salesOrder)
    {
        var order = salesOrder.ToOrder();
        var result = await _orderService.AddAsync(order);
        return result;
    }

    private async Task ProcessPayment(SalesOrder salesOrder)
    {
        var authValid = await _paymentService.IsAuthValidAsync(salesOrder.Payment);
        if (!authValid)
            throw new PaymentNotAuthorizedException(
                $"Payment for SalesOrder: {salesOrder.Id} has not been authorized.  Payment can not be processed");

        var transactionId = await _paymentService.CollectPaymentAsync(salesOrder.Payment);
        _logger.LogInformation("Payment processed for Sales Order: {salesOrder.Id}, TransactionId: {transactionId}", salesOrder.Id, transactionId);
    }

    private async Task<bool> VerifyInventoryAvailable(SalesOrder salesOrder)
    {
        //TODO Implement Backorder functionality if enabled
        var skus = salesOrder.Cart.Items.Select(i => i.Sku);

        var status = await _inventoryService.IsAvailable(skus);
        return status.AllAvailable;
    }
}

public interface ICheckoutService
{
    Task<OrderResult> CheckoutAsync(SalesOrder salesOrder);
}
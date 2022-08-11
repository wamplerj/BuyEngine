using BuyEngine.Checkout.Payment;
using BuyEngine.Checkout.Shipping;
using BuyEngine.Customer;

namespace BuyEngine.Checkout;

public class SalesOrder
{
    public SalesOrder(Cart cart, SalesOrderAddress billTo, SalesOrderAddress shipTo, ShippingMethod shippingMethod,
        PaymentInformation payment)
    {
        Id = Guid.NewGuid();
        Cart = cart;
        BillTo = billTo;
        ShipTo = shipTo;
        ShippingMethod = shippingMethod;
        Payment = payment;
    }

    public Guid Id { get; set; }

    public Cart Cart { get; }
    public SalesOrderAddress BillTo { get; }
    public SalesOrderAddress ShipTo { get; }
    public ShippingMethod ShippingMethod { get; }
    public PaymentInformation Payment { get; }
}

public static class SalesOrderMapper
{
    public static Order ToOrder(this SalesOrder salesOrder) => new();
}

public class SalesOrderAddress : Address
{
    public string Customer { get; set; }
    public string EmailAddress { get; set; }
    public string PhoneNumber { get; set; }
}
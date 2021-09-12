using System;
using System.Threading.Tasks;
using BuyEngine.Checkout.Payment;
using BuyEngine.Checkout.Shipping;
using BuyEngine.Common;

namespace BuyEngine.Checkout
{
    public class SalesOrderBuilder
    {
        private readonly IModelValidator<Cart> _cartValidator;
        private readonly IModelValidator<SalesOrderAddress> _addressValidator;
        private readonly IModelValidator<ShippingMethod> _shippingValidator;
        private readonly IModelValidator<PaymentInformation> _paymentValidator;
        private readonly Task _workTask;

        public SalesOrderBuilder(IModelValidator<Cart> cartValidator,
            IModelValidator<SalesOrderAddress> addressValidator,
            IModelValidator<ShippingMethod> shippingValidator, IModelValidator<PaymentInformation> paymentValidator)
        {
            _cartValidator = cartValidator;
            _addressValidator = addressValidator;
            _shippingValidator = shippingValidator;
            _paymentValidator = paymentValidator ?? throw new ArgumentNullException(nameof(paymentValidator));

            _workTask = Task.FromResult(0);
        }

        public Cart Cart { get; private set; }

        public SalesOrderAddress ShipTo { get; private set; }
        public SalesOrderAddress BillTo { get; private set; }
        public ShippingMethod ShippingMethod { get; private set; }
        public PaymentInformation PaymentInformation { get; private set; }

        public SalesOrderBuilder SetCart(Cart cart)
        {
            if (cart == null) return this;
            Cart = cart;

            return this;
        }

        public SalesOrderBuilder SetShipTo(SalesOrderAddress shipTo)
        {
            if (shipTo == null) return this;
            ShipTo = shipTo;

            return this;
        }

        public SalesOrderBuilder SetBillTo(SalesOrderAddress billTo)
        {
            if (billTo == null) return this;
            BillTo = billTo;

            return this;
        }

        public SalesOrderBuilder SetShippingMethod(ShippingMethod shippingMethod)
        {
            if (shippingMethod == null) return this;
            ShippingMethod = shippingMethod;

            return this;
        }

        public SalesOrderBuilder SetPayment(PaymentInformation payment)
        {
            if (payment == null) return this;
            PaymentInformation = payment;

            return this;
        }

        public async Task<SalesOrder> BuildAsync()
        {
            await _cartValidator.ThrowIfInvalidAsync(Cart, nameof(Cart));
            await _addressValidator.ThrowIfInvalidAsync(BillTo, nameof(BillTo));
            await _addressValidator.ThrowIfInvalidAsync(ShipTo, nameof(ShipTo));
            await _shippingValidator.ThrowIfInvalidAsync(ShippingMethod, nameof(ShippingMethod));
            await _paymentValidator.ThrowIfInvalidAsync(PaymentInformation, nameof(PaymentInformation));

            return new SalesOrder(Cart, BillTo, ShipTo, ShippingMethod, PaymentInformation);
        }
    }
}
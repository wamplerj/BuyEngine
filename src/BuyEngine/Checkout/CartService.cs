using BuyEngine.Catalog;
using BuyEngine.Common;
using Microsoft.Extensions.Logging;

namespace BuyEngine.Checkout;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly ILogger<CartService> _logger;
    private readonly IProductRepository _productRepository;
    private readonly IModelValidator<Cart> _validator;

    public CartService(ICartRepository cartRepository, IProductRepository productRepository, IModelValidator<Cart> validator,
        ILogger<CartService> logger)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Cart> GetAsync(Guid cartId)
    {
        Guard.Default(cartId, nameof(cartId));

        var cart = await _cartRepository.GetAsync(cartId);
        return cart ?? await CreateAsync(cartId);
    }

    public async Task<Cart> CreateAsync(Guid? cartId)
    {
        cartId ??= Guid.NewGuid();
        _logger.LogInformation("Creating new Cart with Id: {cartId}", cartId);

        var cart = new Cart
        {
            Created = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddMinutes(CatalogConfiguration.CartExpirationInMinutes),
            Id = cartId.Value
        };

        await _cartRepository.Add(cart);
        return cart;
    }

    public async Task<Cart> AddItemAsync(Guid cartId, Guid productId, int quantity)
    {
        Guard.Default(cartId, nameof(cartId));
        Guard.Default(productId, nameof(productId));
        Guard.NegativeOrZero(quantity, nameof(quantity));

        var product = await _productRepository.GetAsync(productId);
        if (product == null)
        {
            _logger.LogWarning("Product with Id: {productId} could not be found", productId);
            throw new ArgumentException($"Product with Id: {productId} could not be found", nameof(productId));
        }

        var cart = await GetAsync(cartId);
        var cartItem = cart.Items.FirstOrDefault(ci => ci.ProductId == productId) ?? new CartItem();

        cartItem.Name = product.Name;
        cartItem.Price = product.Price;
        cartItem.Sku = product.Sku;
        cartItem.Quantity = quantity;

        if (cartItem.IsNew)
        {
            cartItem.Id = Guid.NewGuid();
            cart.Items.Add(cartItem);
            _logger.LogInformation("Adding CartItem {cartItem.Id} for Sku: {cartItem.Sku} to Cart Id: {cart.Id}", cartItem.Id, cartItem.Sku, cart.Id);
        }

        await UpdateAsync(cart);

        _logger.LogInformation("Cart with Id: {cartId} updated with Product: {productId}, Quantity: {quantity}", cartId, productId, quantity);
        return cart;
    }

    public async Task UpdateAsync(Cart cart)
    {
        var result = await _validator.ValidateAsync(cart);
        result.ThrowIfInvalid(nameof(cart));

        cart.Expires = DateTime.UtcNow.AddMinutes(CatalogConfiguration.CartExpirationInMinutes);
        _ = await _cartRepository.Update(cart);
    }

    public async Task<bool> AbandonAsync(Guid cartId)
    {
        var cart = await _cartRepository.GetAsync(cartId);

        if (cart == null)
        {
            _logger.LogWarning("Cart Id: {cartId} could not be found.", cartId);
            return false;
        }

        if (cart.IsExpired)
        {
            _logger.LogInformation("Abandoning Cart ID: {cartId}", cartId);
            return await _cartRepository.Delete(cartId);
        }

        _logger.LogWarning("Cart Id: {cartId} has not expired.  Can not be abandoned", cartId);
        return false;
    }
}

public interface ICartService
{
    Task<Cart> GetAsync(Guid cartId);
    Task<Cart> CreateAsync(Guid? cartId);
    Task<Cart> AddItemAsync(Guid cartId, Guid productId, int quantity);
    Task UpdateAsync(Cart cart);
    Task<bool> AbandonAsync(Guid cartId);
}
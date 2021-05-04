﻿using BuyEngine.Catalog;
using BuyEngine.Common;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BuyEngine.Checkout
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IModelValidator<Cart> _validator;
        private readonly ILogger<CartService> _logger;

        public CartService(ICartRepository cartRepository, IProductRepository productRepository, IModelValidator<Cart> validator, ILogger<CartService> logger)
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
            _logger.LogInformation($"Creating new Cart with Id: {cartId}");

            var cart =  new Cart
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
                _logger.LogWarning($"Product with Id: {productId} could not be found");
                throw new ArgumentException($"Product with Id: {productId} could not be found", nameof(productId));
            }

            var cart = await GetAsync(cartId);
            var cartItem = cart.Items.FirstOrDefault(ci => ci.ProductId == productId) ?? new CartItem();

            cartItem.Name = product.Name;
            cartItem.Price = product.Price;
            cartItem.Sku = product.Sku;
            cartItem.Quantity = quantity;

            cart.Items.Add(cartItem);
            await UpdateAsync(cart);

            _logger.LogInformation($"Cart with Id: {cartId} updated with Product: {productId}, Quantity: {quantity}");
            return cart;
        }

        public async Task UpdateAsync(Cart cart)
        {
            var result = await _validator.ValidateAsync(cart);
            if (result.IsNotValid)
                throw new ValidationException(result, nameof(UpdateAsync));

            cart.Expires = DateTime.UtcNow.AddMinutes(CatalogConfiguration.CartExpirationInMinutes);

            var success = await _cartRepository.Update(cart);
        }

        public async Task<bool> AbandonAsync(Guid cartId)
        {
            _logger.LogInformation($"Removing Cart with Id: {cartId}");

            return await _cartRepository.Delete(cartId);
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
}

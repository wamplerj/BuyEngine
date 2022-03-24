﻿using BuyEngine.Catalog;
using BuyEngine.Common;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NLog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BuyEngine.WebApi.Catalog
{
    [Route("products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly Logger _logger;

        public ProductController(IProductService productService)
        {
            _productService = productService;
            _logger = LogManager.GetCurrentClassLogger();
        }

        [HttpGet]
        public async Task<ActionResult> Get(int page = 1, int pageSize = 25)
        {
            var products = await _productService.GetAllAsync(pageSize, page);

            if (products.Any())
            {
                _logger.Info($"{products.Count} Products were found on Page: {page}, PageSize: {pageSize}");
                _logger.Debug(JsonConvert.SerializeObject(products));
                return Ok(products);
            }

            _logger.Warn($"No Products were found on Page: {page}, PageSize: {pageSize}");
            return NotFound("No Products were found");
        }


        [HttpGet]
        [Route("product/{productId}")]
        public async Task<ActionResult> Get(Guid productId)
        {
            if (productId == default)
            {
                _logger.Info($"ProductId is invalid.  Id must be >= 0");
                return BadRequest($"{productId} is not a valid Product ID");
            }

            var product = await _productService.GetAsync(productId);

            if (product != null)
            {
                _logger.Info($"Product: {productId} was found");
                _logger.Debug(JsonConvert.SerializeObject(product));
                return Ok(product);
            }

            _logger.Info($"ProductId: {productId} was not found.");
            return NotFound($"ProductId: {productId} was not found");
        }

        [HttpGet]
        [Route("product/sku/{sku}")]
        public async Task<ActionResult> Get(string sku)
        {
            if (string.IsNullOrEmpty(sku))
            {
                _logger.Info($"Sku is invalid.  Sku cannot be null or empty");
                return BadRequest($"{sku} is not a valid sku");
            }

            var product = await _productService.GetAsync(sku);

            if (product != null)
                return Ok(product);

            _logger.Info($"Sku: {sku} was not found.");
            return NotFound($"Sku: {sku} was not found.");
        }

        [HttpPost]
        [Route("product")]
        public async Task<ActionResult> Add([FromBody] Product product)
        {
            Guard.Null(product, nameof(product));

            var validationResult = await _productService.ValidateAsync(product, requireUniqueSku:true);
            if (!validationResult.IsValid)
            {
                _logger.Info($"Failed to add Product: {product.Sku} due to validation issues");
                return BadRequest(validationResult.Messages);
            }

            var result = await _productService.AddAsync(product);
            return Created($"/be-api/products/{result}", product);
        }

        [HttpPut]
        [Route("product/{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] Product product)
        {
            Guard.Null(product, nameof(product));
            product.Id = id;

            var validationResult = await _productService.ValidateAsync(product, requireUniqueSku:false);
            if (!validationResult.IsValid)
            {
                _logger.Info($"Failed to update Product: {product.Sku} due to validation issues");
                return BadRequest(validationResult.Messages);
            }

            var result = await _productService.UpdateAsync(product);
            return NoContent();
        }
    }
}
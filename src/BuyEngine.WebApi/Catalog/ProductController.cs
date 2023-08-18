using BuyEngine.Catalog;
using BuyEngine.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BuyEngine.WebApi.Catalog;

[Route("products")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly ILogger<ProductController> _logger;
    private readonly IProductService _productService;

    public ProductController(IProductService productService, ILogger<ProductController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult> Get(int page = 1, int pageSize = 25)
    {
        var products = await _productService.GetAllAsync(pageSize, page);

        if (products.Any())
        {
            _logger.LogInformation("{products.Count} Products were found on Page: {page}, PageSize: {pageSize}", products.Count, page, pageSize);
            return Ok(products);
        }

        _logger.LogWarning("No Products were found on Page: {page}, PageSize: {pageSize}", page, pageSize);
        return NotFound("No Products were found");
    }


    [HttpGet]
    [Route("product/{productId}")]
    public async Task<ActionResult> Get(Guid productId)
    {
        if (productId == default)
        {
            _logger.LogInformation("ProductId is invalid.  Id must be >= 0");
            return BadRequest($"{productId} is not a valid Product ID");
        }

        var product = await _productService.GetAsync(productId);

        if (product != null)
        {
            _logger.LogInformation("Product: {productId} was found", productId);
            return Ok(product);
        }

        _logger.LogInformation("ProductId: {productId} was not found.", productId);
        return NotFound($"ProductId: {productId} was not found");
    }

    [HttpGet]
    [Route("product/sku/{sku}")]
    public async Task<ActionResult> Get(string sku)
    {
        if (string.IsNullOrEmpty(sku))
        {
            _logger.LogInformation("Sku: {sku} is invalid.  Sku cannot be null or empty", sku);
            return BadRequest($"{sku} is not a valid sku");
        }

        var product = await _productService.GetAsync(sku);

        if (product != null)
            return Ok(product);

        _logger.LogInformation("Sku: {sku} was not found.", sku);
        return NotFound($"Sku: {sku} was not found.");
    }

    [HttpPost]
    [Route("product")]
    public async Task<ActionResult> Add([FromBody] Product product)
    {
        Guard.Null(product, nameof(product));

        var validationResult = await _productService.ValidateAsync(product, true);
        if (validationResult.IsInvalid)
        {
            _logger.LogInformation("Failed to add Product: {product.Sku} due to validation issues", product.Sku);
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

        var validationResult = await _productService.ValidateAsync(product, false);
        if (validationResult.IsInvalid)
        {
            _logger.LogInformation("Failed to update Product: {product.Sku} due to validation issues", product.Sku);
            return BadRequest(validationResult.Messages);
        }

        _ = await _productService.UpdateAsync(product);
        return NoContent();
    }
}
using BuyEngine.Catalog;
using BuyEngine.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BuyEngine.WebApi.Catalog
{
    [Route("be-api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger _logger;

        public ProductController(IProductService productService, ILogger logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet]
        [Route("/be-api/products/{productId}")]
        public async Task<ActionResult> Get(int productId)
        {
            if (productId <= 0)
                return BadRequest($"{productId} is not a valid Product ID");

            var product = await _productService.GetAsync(productId);

            if (product == null)
                return NotFound($"ProductId: {productId} was not found");

            return Ok(product);
        }

        [HttpPost]
        [Route("/be-api/products")]
        public async Task<ActionResult> Add([FromBody] Product product)
        {
            try
            {
                var result = await _productService.AddAsync(product);
                //TODO Get url dynamically
                return Created($"/be-api/products/{result}", product);

            }
            catch (ValidationException vex)
            {
                _logger.LogError(vex, $"Failed to add Product: {product.Sku} due to validation issues");

                //TODO Map ValidationResult into ModelStateDictionary
                return BadRequest();
            }
        }
    }
}
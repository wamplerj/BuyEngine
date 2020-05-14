using BuyEngine.Catalog;
using BuyEngine.Common;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System.Threading.Tasks;

namespace BuyEngine.WebApi.Catalog
{
    [Route("be-api/products")]
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
        [Route("/be-api/products/{productId}")]
        public async Task<ActionResult> Get(int productId)
        {
            if (productId <= 0)
            {
                _logger.Info($"ProductId is invalid.  Id must be >= 0");
                return BadRequest($"{productId} is not a valid Product ID");
            }

            var product = await _productService.GetAsync(productId);

            if (product != null) 
                return Ok(product);

            _logger.Info($"ProductId: {productId} was not found.");
            return NotFound($"ProductId: {productId} was not found");
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
                _logger.Error(vex, $"Failed to add Product: {product.Sku} due to validation issues");

                //TODO Map ValidationResult into ModelStateDictionary
                return BadRequest();
            }
        }
    }
}
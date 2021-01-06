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
        [Route("/be-api/product/{productId}")]
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

        [HttpGet]
        [Route("/be-api/product/sku/{sku}")]
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
        [Route("/be-api/product")]
        public ActionResult Add([FromBody] Product product)
        {
            Guard.Null(product, nameof(product));

            var validationResult = _productService.Validate(product, requireUniqueSku:true);
            if (!validationResult.IsValid)
            {
                _logger.Info($"Failed to add Product: {product.Sku} due to validation issues");
                return BadRequest(validationResult.Messages);
            }

            var result = _productService.Add(product);
            //TODO Get url dynamically
            return Created($"/be-api/products/{result}", product);
        }

        [HttpPut]
        [Route("/be-api/product/{id}")]
        public ActionResult Update(int id, [FromBody] Product product)
        {
            Guard.Null(product, nameof(product));
            product.Id = id;

            var validationResult = _productService.Validate(product, requireUniqueSku:false);
            if (!validationResult.IsValid)
            {
                _logger.Info($"Failed to update Product: {product.Sku} due to validation issues");
                return BadRequest(validationResult.Messages);
            }

            var result = _productService.Update(product);
            return NoContent();
        }
        
        
    }
}
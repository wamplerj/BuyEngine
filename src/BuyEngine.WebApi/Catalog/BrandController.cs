using BuyEngine.Catalog.Brands;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System.Threading.Tasks;

namespace BuyEngine.WebApi.Catalog
{
    [Route("be-api/products/brands")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;
        private readonly Logger _logger;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
            _logger = LogManager.GetCurrentClassLogger();
        }

        [HttpGet]
        [Route("{brandId}:int")]
        public async Task<ActionResult> Get(int brandId)
        {
            if (brandId <= 0)
            {
                _logger.Info($"BrandId is invalid.  Id must be >= 0");
                return BadRequest($"{brandId} is not a valid Brand ID");
            }

            var product = await _brandService.GetAsync(brandId);

            if (product != null)
                return Ok(product);

            _logger.Info($"BrandId: {brandId} was not found.");
            return NotFound($"BrandId: {brandId} was not found");
        }

        [HttpPost]
        [Route("/be-api/products/brand")]
        public async Task<ActionResult> Add([FromBody] Brand brand)
        {
            var result = await _brandService.ValidateAsync(brand);
            if (!result.IsValid)
            {
                _logger.Info($"Failed to add Brand: {brand.Name} due to validation issues");
                return BadRequest(result.Messages);
            }
            
            var id = await _brandService.AddAsync(brand);
            var url = Url.Action("Get", id);
            return Created(url, brand);
        }

        [HttpPut]
        [Route("/be-api/products/brand")]
        public async Task<ActionResult> Update([FromBody] Brand brand)
        {
            var result = await _brandService.ValidateAsync(brand);
            if (!result.IsValid)
            {
                _logger.Info($"Failed to update Brand: {brand.Name} due to validation issues");
                return BadRequest(result.Messages);
            }

            var success = await _brandService.UpdateAsync(brand);
            //TODO Do we even need a boolean here?

            var url = Url.Action("Get", brand.Id);
            return Ok(url);
        }

        [HttpDelete]
        [Route("/be-api/products/brand/{brandId}")]
        public async Task<ActionResult> Delete(int brandId)
        {
            if (brandId <= 0)
            {
                _logger.Info($"{brandId} is invalid.  Id must be >= 0");
                return BadRequest($"{brandId} is not a valid Brand ID");
            }

            await _brandService.RemoveAsync(brandId);
            _logger.Info($"BrandId: {brandId} was deleted sucessfully.");

            return Ok();
        }
    }
}

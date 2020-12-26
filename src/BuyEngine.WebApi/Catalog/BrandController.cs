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
        public ActionResult Add([FromBody] Brand brand)
        {
            var result = _brandService.Validate(brand);
            if (!result.IsValid)
            {
                _logger.Info($"Failed to add Brand: {brand.Name} due to validation issues");
                return BadRequest(result.Messages);
            }
            
            var id = _brandService.Add(brand);
            return Created(Url.Action("Get", id), brand);
        }

        [HttpPut]
        [Route("/be-api/products/brand")]
        public ActionResult Update([FromBody] Brand brand)
        {
            var result = _brandService.Validate(brand);
            if (!result.IsValid)
            {
                _logger.Info($"Failed to update Brand: {brand.Name} due to validation issues");
                return BadRequest(result.Messages);
            }

            _brandService.Update(brand);
            var url = Url.Action("Get");
            return Ok(url);
        }

        [HttpDelete]
        [Route("/be-api/products/brands/{brandId}")]
        public ActionResult Delete(int brandId)
        {

            if (brandId <= 0)
            {
                _logger.Info($"{brandId} is invalid.  Id must be >= 0");
                return BadRequest($"{brandId} is not a valid Brand ID");
            }

            _brandService.Remove(brandId);
            _logger.Info($"BrandId: {brandId} was deleted sucessfully.");

            return Ok();
        }
    }
}

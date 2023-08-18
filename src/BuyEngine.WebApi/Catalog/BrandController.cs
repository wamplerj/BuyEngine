using BuyEngine.Catalog.Brands;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BuyEngine.WebApi.Catalog;

[Route("be-api/products/brands")]
[ApiController]
public class BrandController : ControllerBase
{
    private readonly IBrandService _brandService;
    private readonly ILogger<BrandController> _logger;

    public BrandController(IBrandService brandService, ILogger<BrandController> logger)
    {
        _brandService = brandService;
        _logger = logger;
    }

    [HttpGet]
    [Route("{brandId}:guid")]
    public async Task<ActionResult> Get(Guid brandId)
    {
        if (brandId == default)
        {
            _logger.LogInformation("BrandId is invalid.  Id must be a valid guid");
            return BadRequest($"{brandId} is not a valid Brand ID");
        }

        var product = await _brandService.GetAsync(brandId);

        if (product != null)
            return Ok(product);

        _logger.LogInformation($"BrandId: {brandId} was not found.");
        return NotFound($"BrandId: {brandId} was not found");
    }

    [HttpPost]
    [Route("/be-api/products/brand")]
    public async Task<ActionResult> Add([FromBody] Brand brand)
    {
        var result = await _brandService.ValidateAsync(brand);
        if (result.IsInvalid)
        {
            _logger.LogInformation($"Failed to add Brand: {brand.Name} due to validation issues");
            return BadRequest(result.Messages);
        }

        var id = await _brandService.AddAsync(brand);
        var url = Url.Action("Get", id)!;
        return Created(url, brand);
    }

    [HttpPut]
    [Route("/be-api/products/brand")]
    public async Task<ActionResult> Update([FromBody] Brand brand)
    {
        var result = await _brandService.ValidateAsync(brand);
        if (result.IsInvalid)
        {
            _logger.LogInformation($"Failed to update Brand: {brand.Name} due to validation issues");
            return BadRequest(result.Messages);
        }

        _ = await _brandService.UpdateAsync(brand);

        var url = Url.Action("Get", brand.Id);
        return Ok(url);
    }

    [HttpDelete]
    [Route("/be-api/products/brand/{brandId}")]
    public async Task<ActionResult> Delete(Guid brandId)
    {
        if (brandId == default)
        {
            _logger.LogInformation($"{brandId} is invalid.  Id must be a valid guid");
            return BadRequest($"{brandId} is not a valid Brand ID");
        }

        await _brandService.RemoveAsync(brandId);
        _logger.LogInformation($"BrandId: {brandId} was deleted successfully.");

        return Ok();
    }
}
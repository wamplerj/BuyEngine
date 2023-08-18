using BuyEngine.Catalog.Suppliers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BuyEngine.WebApi.Catalog;

[Route("be-api/products/suppliers")]
[ApiController]
public class SupplierController : ControllerBase
{
    private readonly ILogger<SupplierController> _logger;
    private readonly ISupplierService _supplierService;

    public SupplierController(ISupplierService supplierService, ILogger<SupplierController> logger)
    {
        _supplierService = supplierService;
        _logger = logger;
    }

    [HttpGet]
    [Route("{supplierId}:int")]
    public async Task<ActionResult> Get(Guid supplierId)
    {
        if (supplierId == default)
        {
            _logger.LogInformation("SupplierId is invalid.  Id must be >= 0");
            return BadRequest($"{supplierId} is not a valid Supplier ID");
        }

        var product = await _supplierService.GetAsync(supplierId);

        if (product != null)
            return Ok(product);

        _logger.LogInformation("SupplierId: {supplierId} was not found.", supplierId);
        return NotFound($"SupplierId: {supplierId} was not found");
    }

    [HttpPost]
    [Route("/be-api/products/suppliers")]
    public async Task<ActionResult> Add([FromBody] Supplier supplier)
    {
        var result = await _supplierService.ValidateAsync(supplier);
        if (result.IsInvalid)
        {
            _logger.LogInformation("Failed to add Supplier: {supplier.Name} due to validation issues", supplier.Name);
            return BadRequest(result.Messages);
        }

        var id = await _supplierService.AddAsync(supplier);
        return Created(Url.Action("Get", id), supplier);
    }

    [HttpPut]
    [Route("/be-api/products/suppliers")]
    public async Task<ActionResult> Update([FromBody] Supplier supplier)
    {
        var result = await _supplierService.ValidateAsync(supplier);
        if (result.IsInvalid)
        {
            _logger.LogInformation("Failed to update Supplier: {supplier.Name} due to validation issues", supplier.Name);
            return BadRequest(result.Messages);
        }

        await _supplierService.UpdateAsync(supplier);
        var url = Url.Action("Get");
        return Ok(url);
    }

    [HttpDelete]
    [Route("/be-api/products/suppliers/{supplierId}")]
    public async Task<ActionResult> Delete(Guid supplierId)
    {
        if (supplierId == default)
        {
            _logger.LogInformation("SupplierId is invalid.  Id must be >= 0");
            return BadRequest($"{supplierId} is not a valid Supplier ID");
        }

        await _supplierService.RemoveAsync(supplierId);
        _logger.LogInformation("SupplierId: {supplierId} was deleted successfully.", supplierId);

        return NoContent();
    }
}
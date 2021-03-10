using BuyEngine.Catalog.Suppliers;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System.Threading.Tasks;

namespace BuyEngine.WebApi.Catalog
{
    [Route("be-api/products/suppliers")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;
        private readonly Logger _logger;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
            _logger = LogManager.GetCurrentClassLogger();
        }

        [HttpGet]
        [Route("{supplierId}:int")]
        public async Task<ActionResult> Get(int supplierId)
        {
            if (supplierId <= 0)
            {
                _logger.Info($"SupplierId is invalid.  Id must be >= 0");
                return BadRequest($"{supplierId} is not a valid Supplier ID");
            }

            var product = await _supplierService.GetAsync(supplierId);

            if (product != null)
                return Ok(product);

            _logger.Info($"SupplierId: {supplierId} was not found.");
            return NotFound($"SupplierId: {supplierId} was not found");
        }

        [HttpPost]
        [Route("/be-api/products/suppliers")]
        public async Task<ActionResult> Add([FromBody] Supplier supplier)
        {
            var result = await _supplierService.ValidateAsync(supplier);
            if (!result.IsValid)
            {
                _logger.Info($"Failed to add Supplier: {supplier.Name} due to validation issues");
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
            if (!result.IsValid)
            {
                _logger.Info($"Failed to update Supplier: {supplier.Name} due to validation issues");
                return BadRequest(result.Messages);
            }

            await _supplierService.UpdateAsync(supplier);
            var url = Url.Action("Get");
            return Ok(url);
        }

        [HttpDelete]
        [Route("/be-api/products/suppliers/{supplierId}")]
        public async Task<ActionResult> Delete(int supplierId)
        {

            if (supplierId <= 0)
            {
                _logger.Info($"SupplierId is invalid.  Id must be >= 0");
                return BadRequest($"{supplierId} is not a valid Supplier ID");
            }

            await _supplierService.RemoveAsync(supplierId);
            _logger.Info($"SupplierId: {supplierId} was deleted sucessfully.");

            return NoContent();
        }
    }
}

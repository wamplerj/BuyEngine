using BuyEngine.Catalog.Suppliers;
using BuyEngine.Common;
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
        public ActionResult Add([FromBody] Supplier supplier)
        {
            try
            {
                var id = _supplierService.Add(supplier);
                return Created(Url.Action("Get", id), supplier);

            }
            catch (ValidationException vex)
            {
                _logger.Error(vex, $"Failed to add Supplier: {supplier.Name} due to validation issues");

                //TODO: Map ValidationResult into ModelStateDictionary
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("/be-api/products/suppliers")]
        public ActionResult Update([FromBody] Supplier supplier)
        {
            try
            {
                _supplierService.Update(supplier);
                var url = Url.Action("Get");
                return Ok(url);

            }
            catch (ValidationException vex)
            {
                _logger.Error(vex, $"Failed to update Supplier: {supplier.Name} due to validation issues");

                //TODO: Map ValidationResult into ModelStateDictionary
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("/be-api/products/suppliers/{supplierId}")]
        public ActionResult Delete(int supplierId)
        {

            if (supplierId <= 0)
            {
                _logger.Info($"SupplierId is invalid.  Id must be >= 0");
                return BadRequest($"{supplierId} is not a valid Supplier ID");
            }

            _supplierService.Remove(supplierId);
            _logger.Info($"SupplierId: {supplierId} was deleted sucessfully.");

            return Ok();
        }
    }
}

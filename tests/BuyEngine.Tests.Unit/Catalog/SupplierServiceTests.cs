using BuyEngine.Catalog.Suppliers;
using BuyEngine.Common;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyEngine.Tests.Unit.Catalog
{
    public class SupplierServiceTests
    {
        private Mock<ISupplierRepository> _supplierRepository;
        private SupplierService _supplierService;

        [SetUp]
        public void SetUp()
        {
            _supplierRepository = new Mock<ISupplierRepository>();

            _supplierService = new SupplierService(_supplierRepository.Object, new SupplierValidator());
        }

        [Test]
        public async Task Getting_A_Known_Supplier_By_Id_Returns_Successfully()
        {
            _supplierRepository.Setup(sr => sr.GetAsync(It.IsAny<int>())).ReturnsAsync(new Supplier());
            var result = await _supplierService.GetAsync(1);
            
            _supplierRepository.Verify(sr => sr.GetAsync(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public async Task Getting_An_Unknown_Supplier_By_Id_Returns_Null()
        {
            _supplierRepository.Setup(sr => sr.GetAsync(It.IsAny<int>())).ReturnsAsync((Supplier) null);
            var result = await _supplierService.GetAsync(1);

            _supplierRepository.Verify(sr => sr.GetAsync(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public async Task Getting_All_Suppliers_Returns_First_Page_Only()
        {
            _supplierRepository.Setup(sr => sr.GetAllAsync(5, 0)).ReturnsAsync(new List<Supplier>());
            var result = await _supplierService.GetAllAsync(5, 0);

            _supplierRepository.Verify(sr => sr.GetAllAsync(5, 0), Times.Once);
        }

        [Test]
        public async Task Valid_Supplier_Validates_Successfully()
        {
            var supplier = new Supplier() {Name = "Test", Notes = "Blah"};

            var result = await _supplierService.IsValidAsync(supplier);
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task Invalid_Supplier_Fails_Validation()
        {
            var supplierService = new SupplierService(null, new SupplierValidator());
            var result = await supplierService.ValidateAsync(new Supplier());

            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Messages.Count, Is.EqualTo(1));
            Assert.That(result.Messages.First().Value, Contains.Substring("Name is Required"));
        }

        [Test]
        public async Task Adding_A_Valid_Supplier_Succeeds()
        {
            _supplierRepository.Setup(sr => sr.AddAsync(It.IsAny<Supplier>())).ReturnsAsync(1);
            var supplier = new Supplier() { Name = "Test" };
            await _supplierService.AddAsync(supplier);

            _supplierRepository.Verify(sr => sr.AddAsync(It.IsAny<Supplier>()), Times.Once);
        }

        [Test]
        public async Task Adding_An_Invalid_Supplier_Fails()
        {
            var supplier = new Supplier();
            Assert.ThrowsAsync<ValidationException>(async () => await _supplierService.AddAsync(supplier));
        }

        [Test]
        public async Task Updating_A_Valid_Supplier_Succeeds()
        {
            _supplierRepository.Setup(sr => sr.UpdateAsync(It.IsAny<Supplier>())).ReturnsAsync(true);

            var supplier = new Supplier() { Name = "Test" };
            await _supplierService.UpdateAsync(supplier);

            _supplierRepository.Verify(sr => sr.UpdateAsync(It.IsAny<Supplier>()), Times.Once);
        }

        [Test]
        public async Task Updating_An_Invalid_Supplier_Fails()
        {
            var supplier = new Supplier();
            Assert.ThrowsAsync<ValidationException>(async () => await _supplierService.UpdateAsync(supplier));
        }

        [Test]
        public async Task Removing_An_Existing_Supplier_Succeeds()
        {
            await _supplierService.RemoveAsync(new Supplier() {Id = 1});
            _supplierRepository.Verify(sr => sr.RemoveAsync(It.IsAny<Supplier>()), Times.Once);

        }

        [Test]
        public async Task Removing_An_Existing_Supplier_By_Id_Succeeds()
        {
            _supplierRepository.Setup(sr => sr.GetAsync(It.IsAny<int>())).ReturnsAsync(new Supplier() {Id = 1});

            await _supplierService.RemoveAsync(1);
            _supplierRepository.Verify(sr => sr.RemoveAsync(It.IsAny<Supplier>()), Times.Once);
        }
    }
}

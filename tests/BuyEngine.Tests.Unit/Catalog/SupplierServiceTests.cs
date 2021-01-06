using BuyEngine.Catalog;
using BuyEngine.Catalog.Suppliers;
using BuyEngine.Common;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyEngine.Tests.Unit.Catalog
{
    public class SupplierServiceTests
    {
        private CatalogDbContext _catalogDbContext;
        private SupplierService _supplierService;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<CatalogDbContext>()
                .UseInMemoryDatabase($"{nameof(SupplierServiceTests)}-{Guid.NewGuid()}").Options;
            _catalogDbContext = new CatalogDbContext(options);

            _supplierService = new SupplierService(_catalogDbContext, new SupplierValidator());
        }

        [Test]
        public void Getting_A_Known_Supplier_By_Id_Returns_Successfully()
        {
            _catalogDbContext.Suppliers.Add(new Supplier() {Id=1,Name = "Test", Notes = "TestNotes", WebsiteUrl = "http://someurl"});
            _catalogDbContext.SaveChanges();

            var result = _supplierService.Get(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Name, Is.EqualTo("Test"));
            Assert.That(result.Notes, Is.EqualTo("TestNotes"));
            Assert.That(result.WebsiteUrl, Is.EqualTo("http://someurl"));
        }

        [Test]
        public void Getting_An_Unknown_Supplier_By_Id_Returns_Null()
        {
            var result = _supplierService.Get(1);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task Getting_All_Suppliers_Returns_First_Page_Only()
        {
            var suppliers = GetSuppliersForPaging();
            foreach (var supplier in suppliers)
            {
                await _catalogDbContext.Suppliers.AddAsync(supplier);
            }

            await _catalogDbContext.SaveChangesAsync();

            var result = await _supplierService.GetAllAsync(5, 0);
            Assert.That(result.Count, Is.EqualTo(5));
            Assert.That(result.First().Id, Is.EqualTo(1));
            Assert.That(result.Last().Id, Is.EqualTo(5));
        }

        [Test]
        public async Task Getting_All_Suppliers_Returns_Second_Page_Only()
        {
            var suppliers = GetSuppliersForPaging();
            foreach (var supplier in suppliers)
            {
                await _catalogDbContext.Suppliers.AddAsync(supplier);
            }

            await _catalogDbContext.SaveChangesAsync();

            var result = await _supplierService.GetAllAsync(5, 1);
            Assert.That(result.Count, Is.EqualTo(5));
            Assert.That(result.First().Id, Is.EqualTo(6));
            Assert.That(result.Last().Id, Is.EqualTo(10));
        }

        [Test]
        public async Task Getting_All_Suppliers_Third_Page_Returns_No_Results()
        {
            var suppliers = GetSuppliersForPaging();
            foreach (var supplier in suppliers)
            {
                await _catalogDbContext.Suppliers.AddAsync(supplier);
            }

            await _catalogDbContext.SaveChangesAsync();

            var result = _supplierService.GetAll(5, 2);
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void Valid_Supplier_Validates_Successfully()
        {
            var supplierService = new SupplierService(null, new SupplierValidator());
            var supplier = new Supplier()
            {
                Name = "Test"
            };

            var result = supplierService.IsValid(supplier);

            Assert.That(result, Is.True);
        }

        [Test]
        public void Invalid_Supplier_Fails_Validation()
        {
            var supplierService = new SupplierService(null, new SupplierValidator());
            var result = supplierService.Validate(new Supplier());

            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Messages.Count, Is.EqualTo(1));
            Assert.That(result.Messages.First().Value, Contains.Substring("Name is Required"));
        }

        [Test]
        public void Adding_A_Valid_Supplier_Succeeds()
        {
            var supplier = new Supplier() { Name = "Test" };
            _supplierService.Add(supplier);

            var result = _catalogDbContext.Suppliers.Find(1);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
        }

        [Test]
        public void Adding_An_Invalid_Supplier_Fails()
        {
            var supplier = new Supplier();
            Assert.Throws<ValidationException>(() => _supplierService.Add(supplier));
        }

        [Test]
        public void Updating_A_Valid_Supplier_Succeeds()
        {
            var supplier = new Supplier() { Id = 1, Name = "Test" };
            _supplierService.Add(supplier);

            supplier.Name = "Update Test";
            _supplierService.Update(supplier);

            var result = _catalogDbContext.Suppliers.Find(1);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Name, Is.EqualTo("Update Test"));
        }

        [Test]
        public void Updating_An_Invalid_Supplier_Fails()
        {
            var supplier = new Supplier();
            Assert.Throws<ValidationException>(() => _supplierService.Update(supplier));
        }

        [Test]
        public void Removing_An_Existing_Supplier_Succeeds()
        {
            var supplier = new Supplier() {Name = "Remove"};
            _catalogDbContext.Suppliers.Add(supplier);
            _catalogDbContext.SaveChanges();

            try
            {
                _supplierService.Remove(supplier);
            }
            catch (Exception)
            {
                Assert.Fail();
                return;
            }

            Assert.Pass();
        }

        [Test]
        public void Removing_An_Existing_Supplier_By_Id_Succeeds()
        {
            var supplier = new Supplier() { Id = 1, Name = "Remove" };
            _catalogDbContext.Suppliers.Add(supplier);
            _catalogDbContext.SaveChanges();


            try
            {
                _supplierService.Remove(1);
            }
            catch (Exception)
            {
                Assert.Fail();
                return;
            }

            Assert.Pass();
        }
        
        private IEnumerable<Supplier> GetSuppliersForPaging()
        {
            var suppliers = new List<Supplier>();
            for (var i = 1; i <= 10; i++)
            {
                var supplier = new Supplier()
                {
                    Id = i,
                    Name = $"Supplier {i}"
                };
                suppliers.Add(supplier);
            }

            return suppliers;
        }

    }
}

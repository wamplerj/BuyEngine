using BuyEngine.Catalog;
using BuyEngine.Catalog.Brands;
using BuyEngine.Common;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyEngine.Tests.Unit.Catalog
{
    public class BrandServiceTests
    {
        private CatalogDbContext _catalogDbContext;
        private BrandService _brandService;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<CatalogDbContext>()
                .UseInMemoryDatabase($"{nameof(BrandServiceTests)}-{Guid.NewGuid()}").Options;
            _catalogDbContext = new CatalogDbContext(options);

            _brandService = new BrandService(_catalogDbContext, new BrandValidator());
        }

        [Test]
        public void Getting_A_Known_Brand_By_Id_Returns_Successfully()
        {
            _catalogDbContext.Brands.Add(new Brand() {Id=1,Name = "Test", Notes = "TestNotes", WebsiteUrl = "http://someurl"});
            _catalogDbContext.SaveChanges();

            var result = _brandService.Get(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Name, Is.EqualTo("Test"));
            Assert.That(result.Notes, Is.EqualTo("TestNotes"));
            Assert.That(result.WebsiteUrl, Is.EqualTo("http://someurl"));
        }

        [Test]
        public void Getting_An_Unknown_Brand_By_Id_Returns_Null()
        {
            var result = _brandService.Get(1);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task Getting_All_Brands_Returns_First_Page_Only()
        {
            var brands = GetBrandsForPaging();
            foreach (var brand in brands)
            {
                await _catalogDbContext.Brands.AddAsync(brand);
            }

            await _catalogDbContext.SaveChangesAsync();

            var result = await _brandService.GetAllAsync(5, 0);
            Assert.That(result.Count, Is.EqualTo(5));
            Assert.That(result.First().Id, Is.EqualTo(1));
            Assert.That(result.Last().Id, Is.EqualTo(5));
        }

        [Test]
        public async Task Getting_All_Brands_Returns_Second_Page_Only()
        {
            var brands = GetBrandsForPaging();
            foreach (var brand in brands)
            {
                await _catalogDbContext.Brands.AddAsync(brand);
            }

            await _catalogDbContext.SaveChangesAsync();

            var result = await _brandService.GetAllAsync(5, 1);
            Assert.That(result.Count, Is.EqualTo(5));
            Assert.That(result.First().Id, Is.EqualTo(6));
            Assert.That(result.Last().Id, Is.EqualTo(10));
        }

        [Test]
        public async Task Getting_All_Brands_Third_Page_Returns_No_Results()
        {
            var brands = GetBrandsForPaging();
            foreach (var brand in brands)
            {
                await _catalogDbContext.Brands.AddAsync(brand);
            }

            await _catalogDbContext.SaveChangesAsync();

            var result = await _brandService.GetAllAsync(5, 2);
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void Valid_Brand_Validates_Successfully()
        {
            var brandService = new BrandService(null, new BrandValidator());
            var brand = new Brand()
            {
                Name = "Test"
            };

            var result = brandService.IsValid(brand);

            Assert.That(result, Is.True);
        }

        [Test]
        public void Invalid_Brands_Fails_Validation()
        {
            var brandService = new BrandService(null, new BrandValidator());
            var result = brandService.Validate(new Brand());

            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Messages.Count, Is.EqualTo(1));
            Assert.That(result.Messages.First().Value, Contains.Substring("Name is Required"));
        }

        [Test]
        public void Adding_A_Valid_Brand_Succeeds()
        {
            var brand = new Brand() { Name = "Test" };
            _brandService.Add(brand);

            var result = _catalogDbContext.Brands.Find(1);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
        }

        [Test]
        public void Adding_An_Invalid_Brand_Fails()
        {
            var brand = new Brand();
            Assert.Throws<ValidationException>(() => _brandService.Add(brand));
        }

        [Test]
        public void Updating_A_Valid_Brand_Succeeds()
        {
            var brand = new Brand() { Id = 1, Name = "Test" };
            _brandService.Add(brand);

            brand.Name = "Update Test";
            _brandService.Update(brand);

            var result = _catalogDbContext.Brands.Find(1);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Name, Is.EqualTo("Update Test"));
        }

        [Test]
        public void Updating_An_Invalid_Brand_Fails()
        {
            var brand = new Brand();
            Assert.Throws<ValidationException>(() => _brandService.Update(brand));
        }

        [Test]
        public void Removing_An_Existing_Brand_Succeeds()
        {
            var brand = new Brand() {Name = "Remove"};
            _catalogDbContext.Brands.Add(brand);
            _catalogDbContext.SaveChanges();

            try
            {
                _brandService.Remove(brand);
            }
            catch (Exception)
            {
                Assert.Fail();
                return;
            }

            Assert.Pass();
        }

        [Test]
        public void Removing_An_Existing_Brand_By_Id_Succeeds()
        {
            var brand = new Brand() { Id = 1, Name = "Remove" };
            _catalogDbContext.Brands.Add(brand);
            _catalogDbContext.SaveChanges();


            try
            {
                _brandService.Remove(1);
            }
            catch (Exception)
            {
                Assert.Fail();
                return;
            }

            Assert.Pass();
        }


        private IEnumerable<Brand> GetBrandsForPaging()
        {
            var brands = new List<Brand>();
            for (var i = 1; i <= 10; i++)
            {
                var brand = new Brand()
                {
                    Id = i,
                    Name = $"Brand {i}"
                };
                brands.Add(brand);
            }

            return brands;
        }

    }
}

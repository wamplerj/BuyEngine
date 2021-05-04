using BuyEngine.Catalog.Brands;
using BuyEngine.Common;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyEngine.Tests.Unit.Catalog
{
    public class BrandServiceTests
    {
        private Mock<IBrandRepository> _brandRepository;
        private BrandService _brandService;

        [SetUp]
        public void SetUp()
        {
            _brandRepository = new Mock<IBrandRepository>();
            _brandService = new BrandService(_brandRepository.Object, new BrandValidator());
        }

        [Test]
        public async Task Getting_A_Known_Brand_By_Id_Returns_Successfully()
        {

            var id = Guid.NewGuid();
            _brandRepository.Setup(br => br.GetAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new Brand() {Id=id,Name = "Test", Notes = "TestNotes", WebsiteUrl = "http://someurl"});

            var result = await _brandService.GetAsync(id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(id));
            Assert.That(result.Name, Is.EqualTo("Test"));
            Assert.That(result.Notes, Is.EqualTo("TestNotes"));
            Assert.That(result.WebsiteUrl, Is.EqualTo("http://someurl"));
        }

        [Test]
        public async Task Getting_An_Unknown_Brand_By_Id_Returns_Null()
        {
            var result = await _brandService.GetAsync(Guid.NewGuid());

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task Getting_All_Brands_Returns_Results()
        {
          
            var result = await _brandService.GetAllAsync(5, 0);
            _brandRepository.Verify(br => br.GetAllAsync(5, 0), Times.Once);

        }

        [Test]
        public async Task Valid_Brand_Validates_Successfully()
        {
            var brandService = new BrandService(null, new BrandValidator());
            var brand = new Brand()
            {
                Name = "Test"
            };

            var result = await brandService.IsValidAsync(brand);

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task Invalid_Brands_Fails_Validation()
        {
            var brandService = new BrandService(null, new BrandValidator());
            var result = await brandService.ValidateAsync(new Brand());

            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Messages.Count, Is.EqualTo(1));
            Assert.That(result.Messages.First().Value, Contains.Substring("Name is Required"));
        }

        [Test]
        public async Task Adding_A_Valid_Brand_Succeeds()
        {
            var brand = new Brand() { Name = "Test" };
            await _brandService.AddAsync(brand);

            _brandRepository.Verify(br => br.AddAsync(brand), Times.Once);
        }

        [Test]
        public void Adding_An_Invalid_Brand_Fails()
        {
            var brand = new Brand();
            Assert.ThrowsAsync<ValidationException>(async () => await _brandService.AddAsync(brand));
            _brandRepository.Verify(br => br.AddAsync(It.IsAny<Brand>()), Times.Never);
        }

        [Test]
        public async Task Updating_A_Valid_Brand_Succeeds()
        {
            _brandRepository.Setup(br => br.UpdateAsync(It.IsAny<Brand>())).ReturnsAsync(true);
            await _brandService.UpdateAsync(new Brand() {Name = "Test"});

            _brandRepository.Verify(br => br.UpdateAsync(It.IsAny<Brand>()), Times.Once);
        }

        [Test]
        public async Task Updating_An_Invalid_Brand_Fails()
        {
            var brand = new Brand();
            Assert.ThrowsAsync<ValidationException>(async () => await _brandService.UpdateAsync(brand));
        }

        [Test]
        public async Task Removing_An_Existing_Brand_Succeeds()
        {
            var id = Guid.NewGuid();
            await _brandService.RemoveAsync(new Brand() {Id = id});
            _brandRepository.Verify(br => br.RemoveAsync(It.IsAny<Brand>()), Times.Once);
        }

        [Test]
        public async Task Removing_An_Existing_Brand_By_Id_Succeeds()
        {
            var id = Guid.NewGuid();
            _brandRepository.Setup(br => br.GetAsync(It.IsAny<Guid>())).ReturnsAsync(new Brand() {Id = id});

            await _brandService.RemoveAsync(id);
            _brandRepository.Verify(br => br.RemoveAsync(It.IsAny<Brand>()), Times.Once);

        }
    }
}

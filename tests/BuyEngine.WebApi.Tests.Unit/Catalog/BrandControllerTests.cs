using BuyEngine.Catalog.Brands;
using BuyEngine.Common;
using BuyEngine.WebApi.Catalog;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BuyEngine.WebApi.Tests.Unit.Catalog
{
    public class BrandControllerTests
    {

        private BrandController _controller;
        private Mock<IBrandService> _brandService;

        [SetUp]
        public void Setup()
        {
            _brandService = new Mock<IBrandService>();
            _controller = new BrandController(_brandService.Object);
        }

        [Test]
        public async Task Getting_A_Valid_Supplier_ById_Returns_Ok()
        {
            var guid = Guid.NewGuid();
            _brandService.Setup(ps => ps.GetAsync(guid)).ReturnsAsync(new Brand() {Id = guid});

            var result = await _controller.Get(guid);
            Assert.That(result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public async Task Getting_An_Invalid_Supplier_ById_Returns_BadRequest()
        {
            var result = await _controller.Get(Guid.Empty);
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task Getting_An_Unknown_Supplier_ById_Returns_NotFound()
        {
            var guid = Guid.NewGuid();
            _brandService.Setup(ps => ps.GetAsync(guid)).ReturnsAsync(null as Brand);
            var result = await _controller.Get(guid);
            Assert.That(result, Is.TypeOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task Creating_A_Valid_Supplier_Returns_Created()
        {

            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            Expression<Func<IUrlHelper, string>> urlSetup
                = url => url.Action(It.Is<UrlActionContext>(uac => uac.Action == "Get"));
            mockUrlHelper.Setup(urlSetup).Returns("a/mock/url/for/testing").Verifiable();

            _controller.Url = mockUrlHelper.Object;

            _brandService.Setup(ps => ps.AddAsync(It.IsAny<Brand>())).ReturnsAsync(Guid.NewGuid);
            _brandService.Setup(ss => ss.ValidateAsync(It.IsAny<Brand>())).ReturnsAsync(new ValidationResult() { });

            var result = await _controller.Add(new Brand());
            Assert.That(result, Is.TypeOf<CreatedResult>());
            _brandService.Verify(ss => ss.AddAsync(It.IsAny<Brand>()), Times.Once);
        }

        [Test]
        public async Task Creating_An_Invalid_Supplier_Returns_BadRequest()
        {

            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            Expression<Func<IUrlHelper, string>> urlSetup
                = url => url.Action(It.Is<UrlActionContext>(uac => uac.Action == "Get"));
            mockUrlHelper.Setup(urlSetup).Returns("a/mock/url/for/testing").Verifiable();

            _controller.Url = mockUrlHelper.Object;

            var validationResult = new ValidationResult()
            {
                Messages = {{"Bad Value", "Oops"}}
            };

            _brandService.Setup(ps => ps.AddAsync(It.IsAny<Brand>())).ReturnsAsync(Guid.NewGuid);
            _brandService.Setup(ss => ss.ValidateAsync(It.IsAny<Brand>())).ReturnsAsync(validationResult);

            var result = await _controller.Add(new Brand());
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
            _brandService.Verify(ss => ss.AddAsync(It.IsAny<Brand>()), Times.Never);
        }

        [Test]
        public async Task Updating_A_Valid_Supplier_Returns_Ok()
        {

            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            Expression<Func<IUrlHelper, string>> urlSetup
                = url => url.Action(It.Is<UrlActionContext>(uac => uac.Action == "Get"));
            mockUrlHelper.Setup(urlSetup).Returns("a/mock/url/for/testing").Verifiable();

            _controller.Url = mockUrlHelper.Object;

            _brandService.Setup(ps => ps.UpdateAsync(It.IsAny<Brand>()));
            _brandService.Setup(ss => ss.ValidateAsync(It.IsAny<Brand>())).ReturnsAsync(new ValidationResult() { });

            var result = await _controller.Update(new Brand());
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            _brandService.Verify(ss => ss.UpdateAsync(It.IsAny<Brand>()), Times.Once);
        }

        [Test]
        public async Task Updating_An_Invalid_Supplier_Returns_BadRequest()
        {

            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            Expression<Func<IUrlHelper, string>> urlSetup
                = url => url.Action(It.Is<UrlActionContext>(uac => uac.Action == "Get"));
            mockUrlHelper.Setup(urlSetup).Returns("a/mock/url/for/testing").Verifiable();

            _controller.Url = mockUrlHelper.Object;

            var validationResult = new ValidationResult()
            {
                Messages = { { "Bad Value", "Oops" } }
            };

            _brandService.Setup(ps => ps.AddAsync(It.IsAny<Brand>())).ReturnsAsync(Guid.NewGuid);
            _brandService.Setup(ss => ss.ValidateAsync(It.IsAny<Brand>())).ReturnsAsync(validationResult);

            var result = await _controller.Update(new Brand());
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
            _brandService.Verify(ss => ss.UpdateAsync(It.IsAny<Brand>()), Times.Never);
        }
    }
}
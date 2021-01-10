using BuyEngine.Catalog.Brands;
using BuyEngine.Common;
using BuyEngine.WebApi.Catalog;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using System;
using System.Linq.Expressions;

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
        public void Getting_A_Valid_Supplier_ById_Returns_Ok()
        {
            _brandService.Setup(ps => ps.GetAsync(1)).ReturnsAsync(new Brand() {Id = 1});

            var result = _controller.Get(1).Result;
            Assert.That(result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public void Getting_An_Invalid_Supplier_ById_Returns_BadRequest()
        {
            var result = _controller.Get(0).Result;
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public void Getting_An_Unknown_Supplier_ById_Returns_NotFound()
        {
            _brandService.Setup(ps => ps.GetAsync(1)).ReturnsAsync(null as Brand);
            var result = _controller.Get(123).Result;
            Assert.That(result, Is.TypeOf<NotFoundObjectResult>());
        }

        [Test]
        public void Creating_A_Valid_Supplier_Returns_Created()
        {

            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            Expression<Func<IUrlHelper, string>> urlSetup
                = url => url.Action(It.Is<UrlActionContext>(uac => uac.Action == "Get"));
            mockUrlHelper.Setup(urlSetup).Returns("a/mock/url/for/testing").Verifiable();

            _controller.Url = mockUrlHelper.Object;
            
            _brandService.Setup(ps => ps.Add(It.IsAny<Brand>())).Returns(1);
            _brandService.Setup(ss => ss.Validate(It.IsAny<Brand>())).Returns(new ValidationResult() { });

            var result = _controller.Add(new Brand());
            Assert.That(result, Is.TypeOf<CreatedResult>());
            _brandService.Verify(ss => ss.Add(It.IsAny<Brand>()), Times.Once);
        }

        [Test]
        public void Creating_An_Invalid_Supplier_Returns_BadRequest()
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

            _brandService.Setup(ps => ps.Add(It.IsAny<Brand>())).Returns(1);
            _brandService.Setup(ss => ss.Validate(It.IsAny<Brand>())).Returns(validationResult);

            var result = _controller.Add(new Brand());
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
            _brandService.Verify(ss => ss.Add(It.IsAny<Brand>()), Times.Never);
        }

        [Test]
        public void Updating_A_Valid_Supplier_Returns_Ok()
        {

            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            Expression<Func<IUrlHelper, string>> urlSetup
                = url => url.Action(It.Is<UrlActionContext>(uac => uac.Action == "Get"));
            mockUrlHelper.Setup(urlSetup).Returns("a/mock/url/for/testing").Verifiable();

            _controller.Url = mockUrlHelper.Object;

            _brandService.Setup(ps => ps.Update(It.IsAny<Brand>()));
            _brandService.Setup(ss => ss.Validate(It.IsAny<Brand>())).Returns(new ValidationResult() { });

            var result = _controller.Update(new Brand());
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            _brandService.Verify(ss => ss.Update(It.IsAny<Brand>()), Times.Once);
        }

        [Test]
        public void Updating_An_Invalid_Supplier_Returns_BadRequest()
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

            _brandService.Setup(ps => ps.Add(It.IsAny<Brand>())).Returns(1);
            _brandService.Setup(ss => ss.Validate(It.IsAny<Brand>())).Returns(validationResult);

            var result = _controller.Update(new Brand());
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
            _brandService.Verify(ss => ss.Update(It.IsAny<Brand>()), Times.Never);
        }
    }
}
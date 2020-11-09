using BuyEngine.Catalog.Suppliers;
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
    public class SupplierControllerTests
    {

        private SupplierController _controller;
        private Mock<ISupplierService> _supplierService;

        [SetUp]
        public void Setup()
        {
            _supplierService = new Mock<ISupplierService>();
            _controller = new SupplierController(_supplierService.Object);
        }

        [Test]
        public void Getting_A_Valid_Supplier_ById_Returns_Ok()
        {
            _supplierService.Setup(ps => ps.GetAsync(1)).ReturnsAsync(new Supplier() {Id = 1});

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
            _supplierService.Setup(ps => ps.GetAsync(1)).ReturnsAsync(null as Supplier);
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
            
            _supplierService.Setup(ps => ps.Add(It.IsAny<Supplier>())).Returns(1);
            _supplierService.Setup(ss => ss.Validate(It.IsAny<Supplier>())).Returns(new ValidationResult() { });

            var result = _controller.Add(new Supplier());
            Assert.That(result, Is.TypeOf<CreatedResult>());
            _supplierService.Verify(ss => ss.Add(It.IsAny<Supplier>()), Times.Once);
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

            _supplierService.Setup(ps => ps.Add(It.IsAny<Supplier>())).Returns(1);
            _supplierService.Setup(ss => ss.Validate(It.IsAny<Supplier>())).Returns(validationResult);

            var result = _controller.Add(new Supplier());
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
            _supplierService.Verify(ss => ss.Add(It.IsAny<Supplier>()), Times.Never);
        }

        [Test]
        public void Updating_A_Valid_Supplier_Returns_Ok()
        {

            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            Expression<Func<IUrlHelper, string>> urlSetup
                = url => url.Action(It.Is<UrlActionContext>(uac => uac.Action == "Get"));
            mockUrlHelper.Setup(urlSetup).Returns("a/mock/url/for/testing").Verifiable();

            _controller.Url = mockUrlHelper.Object;

            _supplierService.Setup(ps => ps.Update(It.IsAny<Supplier>()));
            _supplierService.Setup(ss => ss.Validate(It.IsAny<Supplier>())).Returns(new ValidationResult() { });

            var result = _controller.Update(new Supplier());
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            _supplierService.Verify(ss => ss.Update(It.IsAny<Supplier>()), Times.Once);
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

            _supplierService.Setup(ps => ps.Add(It.IsAny<Supplier>())).Returns(1);
            _supplierService.Setup(ss => ss.Validate(It.IsAny<Supplier>())).Returns(validationResult);

            var result = _controller.Update(new Supplier());
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
            _supplierService.Verify(ss => ss.Update(It.IsAny<Supplier>()), Times.Never);
        }
    }
}
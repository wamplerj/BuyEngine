using BuyEngine.Catalog.Suppliers;
using BuyEngine.Common;
using BuyEngine.WebApi.Catalog;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BuyEngine.WebApi.Tests.Unit.Catalog;

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
    public async Task Getting_A_Valid_Supplier_ById_Returns_Ok()
    {
        var id = Guid.NewGuid();
        _supplierService.Setup(ps => ps.GetAsync(id)).ReturnsAsync(new Supplier { Id = id });

        var result = await _controller.Get(id);
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
        var id = Guid.NewGuid();
        _supplierService.Setup(ps => ps.GetAsync(id)).ReturnsAsync(null as Supplier);
        var result = await _controller.Get(id);
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

        _supplierService.Setup(ps => ps.AddAsync(It.IsAny<Supplier>())).ReturnsAsync(Guid.NewGuid);
        _supplierService.Setup(ss => ss.ValidateAsync(It.IsAny<Supplier>())).ReturnsAsync(new ValidationResult());

        var result = await _controller.Add(new Supplier());
        Assert.That(result, Is.TypeOf<CreatedResult>());
        _supplierService.Verify(ss => ss.AddAsync(It.IsAny<Supplier>()), Times.Once);
    }

    [Test]
    public async Task Creating_An_Invalid_Supplier_Returns_BadRequest()
    {
        var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
        Expression<Func<IUrlHelper, string>> urlSetup
            = url => url.Action(It.Is<UrlActionContext>(uac => uac.Action == "Get"));
        mockUrlHelper.Setup(urlSetup).Returns("a/mock/url/for/testing").Verifiable();

        _controller.Url = mockUrlHelper.Object;

        var validationResult = new ValidationResult
        {
            Messages = { { "Bad Value", "Oops" } }
        };

        _supplierService.Setup(ps => ps.AddAsync(It.IsAny<Supplier>())).ReturnsAsync(Guid.NewGuid);
        _supplierService.Setup(ss => ss.ValidateAsync(It.IsAny<Supplier>())).ReturnsAsync(validationResult);

        var result = await _controller.Add(new Supplier());
        Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        _supplierService.Verify(ss => ss.AddAsync(It.IsAny<Supplier>()), Times.Never);
    }

    [Test]
    public async Task Updating_A_Valid_Supplier_Returns_Ok()
    {
        var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
        Expression<Func<IUrlHelper, string>> urlSetup
            = url => url.Action(It.Is<UrlActionContext>(uac => uac.Action == "Get"));
        mockUrlHelper.Setup(urlSetup).Returns("a/mock/url/for/testing").Verifiable();

        _controller.Url = mockUrlHelper.Object;

        _supplierService.Setup(ps => ps.UpdateAsync(It.IsAny<Supplier>()));
        _supplierService.Setup(ss => ss.ValidateAsync(It.IsAny<Supplier>())).ReturnsAsync(new ValidationResult());

        var result = await _controller.Update(new Supplier());
        Assert.That(result, Is.TypeOf<OkObjectResult>());
        _supplierService.Verify(ss => ss.UpdateAsync(It.IsAny<Supplier>()), Times.Once);
    }

    [Test]
    public async Task Updating_An_Invalid_Supplier_Returns_BadRequest()
    {
        var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
        Expression<Func<IUrlHelper, string>> urlSetup
            = url => url.Action(It.Is<UrlActionContext>(uac => uac.Action == "Get"));
        mockUrlHelper.Setup(urlSetup).Returns("a/mock/url/for/testing").Verifiable();

        _controller.Url = mockUrlHelper.Object;

        var validationResult = new ValidationResult
        {
            Messages = { { "Bad Value", "Oops" } }
        };

        _supplierService.Setup(ps => ps.AddAsync(It.IsAny<Supplier>())).ReturnsAsync(Guid.NewGuid);
        _supplierService.Setup(ss => ss.ValidateAsync(It.IsAny<Supplier>())).ReturnsAsync(validationResult);

        var result = await _controller.Update(new Supplier());
        Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        _supplierService.Verify(ss => ss.UpdateAsync(It.IsAny<Supplier>()), Times.Never);
    }
}
using BuyEngine.Catalog;
using BuyEngine.Catalog.Brands;
using BuyEngine.Catalog.Suppliers;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Threading.Tasks;

namespace BuyEngine.Tests.Unit.Catalog;

public class DependencyRegistrationTests
{
    [Test]
    public async Task Catalog_Dependencies_Resolve()
    {
        var services = new ServiceCollection();

        services.AddLogging();
        services.AddCatalogServices(true);

        var provider = services.BuildServiceProvider();

        var productService = provider.GetService<IProductService>();
        Assert.That(productService, Is.Not.Null);
        Assert.That(productService, Is.TypeOf<ProductService>());

        var brandService = provider.GetService<IBrandService>();
        Assert.That(brandService, Is.Not.Null);
        Assert.That(brandService, Is.TypeOf<BrandService>());

        var supplierService = provider.GetService<ISupplierService>();
        Assert.That(supplierService, Is.Not.Null);
        Assert.That(supplierService, Is.TypeOf<SupplierService>());
    }
}
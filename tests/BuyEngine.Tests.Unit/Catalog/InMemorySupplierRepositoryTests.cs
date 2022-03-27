using BuyEngine.Catalog;
using BuyEngine.Catalog.Suppliers;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace BuyEngine.Tests.Unit.Catalog;

public class InMemorySupplierRepositoryTests
{
    private InMemorySupplierRepository _repository;

    [SetUp]
    public async Task SetUp() => _repository = new InMemorySupplierRepository(new InMemoryDataStore(), null);

    [Test]
    public async Task A_Supplier_Can_Be_Added_To_The_Repository()
    {
        var supplier = new Supplier();

        var id = await _repository.AddAsync(supplier);
        Assert.That(id, Is.Not.EqualTo(Guid.Empty));
    }

    [Test]
    public async Task A_Duplicate_Supplier_Can_Not_Be_Added_To_The_Repository()
    {
        var duplicateId = Guid.NewGuid();

        var product1 = new Supplier { Name = "Supplier 1" };
        var product2 = new Supplier { Name = "Supplier 2" };

        var id = await _repository.AddAsync(product1);
        product2.Id = id;

        Assert.ThrowsAsync<ArgumentException>(() => _repository.AddAsync(product2));
    }

    [Test]
    public async Task A_Supplier_Can_Be_Updated_In_The_Repository()
    {
        var supplier = new Supplier { Id = Guid.NewGuid() };

        var id = await _repository.AddAsync(supplier);

        var updated = await _repository.UpdateAsync(new Supplier { Id = Guid.NewGuid() });
        Assert.That(updated, Is.False);

        updated = await _repository.UpdateAsync(new Supplier { Id = id, Name = "Test Supplier" });
        Assert.That(updated, Is.True);
    }

    [Test]
    public async Task A_Supplier_Can_Be_Removed_From_The_Repository()
    {
        var supplier = new Supplier { Id = Guid.NewGuid() };
        var id = await _repository.AddAsync(supplier);

        await _repository.RemoveAsync(supplier);

        var exists = await _repository.GetAsync(supplier.Id) != default;
        Assert.That(exists, Is.False);
    }

    [Test]
    public async Task A_Supplier_Can_Be_Removed_From_The_Repository_By_Id()
    {
        var supplier = new Supplier { Id = Guid.NewGuid() };
        var id = await _repository.AddAsync(supplier);

        await _repository.RemoveAsync(id);

        var exists = await _repository.GetAsync(supplier.Id) != default;
        Assert.That(exists, Is.False);
    }

    [Test]
    public async Task An_Existing_Supplier_Can_Be_Found()
    {
        var supplier = new Supplier { Id = Guid.NewGuid() };
        var id = await _repository.AddAsync(supplier);

        var exists = await _repository.GetAsync(supplier.Id) != default;
        Assert.That(exists, Is.True);
    }

    [Test]
    public async Task All_Suppliers_Can_Be_Returned()
    {
        for (var i = 1; i <= 20; i++)
        {
            var supplier = new Supplier
            {
                Id = Guid.NewGuid(),
                Name = $"TST-Name-{i}"
            };
            await _repository.AddAsync(supplier);
        }

        var suppliers = await _repository.GetAllAsync(10, 1);

        Assert.That(suppliers.Count, Is.EqualTo(10));
        Assert.That(suppliers[4].Name, Is.EqualTo("TST-Name-5"));

        suppliers = await _repository.GetAllAsync(10, 2);
        Assert.That(suppliers.Count, Is.EqualTo(10));
        Assert.That(suppliers[4].Name, Is.EqualTo("TST-Name-15"));
    }
}
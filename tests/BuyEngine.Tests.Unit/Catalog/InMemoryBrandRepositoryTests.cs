using BuyEngine.Catalog;
using BuyEngine.Catalog.Brands;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace BuyEngine.Tests.Unit.Catalog;

public class InMemoryBrandRepositoryTests
{
    private InMemoryBrandRepository _repository;

    [SetUp]
    public async Task SetUp() => _repository = new InMemoryBrandRepository(new InMemoryDataStore(), null);

    [Test]
    public async Task A_Brand_Can_Be_Added_To_The_Repository()
    {
        var brand = new Brand();

        var id = await _repository.AddAsync(brand);
        Assert.That(id, Is.Not.EqualTo(Guid.Empty));
    }

    [Test]
    public async Task A_Duplicate_Brand_Can_Not_Be_Added_To_The_Repository()
    {
        var duplicateId = Guid.NewGuid();

        var product1 = new Brand { Name = "Brand 1" };
        var product2 = new Brand { Name = "Brand 2" };

        var id = await _repository.AddAsync(product1);
        product2.Id = id;

        Assert.ThrowsAsync<ArgumentException>(() => _repository.AddAsync(product2));
    }

    [Test]
    public async Task A_Brand_Can_Be_Updated_In_The_Repository()
    {
        var brand = new Brand { Id = Guid.NewGuid() };

        var id = await _repository.AddAsync(brand);

        var updated = await _repository.UpdateAsync(new Brand { Id = Guid.NewGuid() });
        Assert.That(updated, Is.False);

        updated = await _repository.UpdateAsync(new Brand { Id = id, Name = "Test Brand" });
        Assert.That(updated, Is.True);
    }

    [Test]
    public async Task A_Brand_Can_Be_Removed_From_The_Repository()
    {
        var brand = new Brand { Id = Guid.NewGuid() };
        var id = await _repository.AddAsync(brand);

        await _repository.RemoveAsync(brand);

        var exists = await _repository.GetAsync(brand.Id) != default;
        Assert.That(exists, Is.False);
    }

    [Test]
    public async Task A_Brand_Can_Be_Removed_From_The_Repository_By_Id()
    {
        var brand = new Brand { Id = Guid.NewGuid() };
        var id = await _repository.AddAsync(brand);

        await _repository.RemoveAsync(id);

        var exists = await _repository.GetAsync(brand.Id) != default;
        Assert.That(exists, Is.False);
    }

    [Test]
    public async Task An_Existing_Brand_Can_Be_Found()
    {
        var brand = new Brand { Id = Guid.NewGuid() };
        var id = await _repository.AddAsync(brand);

        var exists = await _repository.GetAsync(brand.Id) != default;
        Assert.That(exists, Is.True);
    }

    [Test]
    public async Task All_Brands_Can_Be_Returned()
    {
        for (var i = 1; i <= 20; i++)
        {
            var brand = new Brand
            {
                Id = Guid.NewGuid(),
                Name = $"TST-Name-{i}"
            };
            await _repository.AddAsync(brand);
        }

        var brands = await _repository.GetAllAsync(10, 1);

        Assert.That(brands.Count, Is.EqualTo(10));
        Assert.That(brands[4].Name, Is.EqualTo("TST-Name-5"));

        brands = await _repository.GetAllAsync(10, 2);
        Assert.That(brands.Count, Is.EqualTo(10));
        Assert.That(brands[4].Name, Is.EqualTo("TST-Name-15"));
    }
}
using BuyEngine.Catalog.Brands;
using BuyEngine.Common;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuyEngine.Tests.Unit.Common;

public class PagedListTests
{
    private List<Brand> _brands;
    private PagedList<Brand> _pageList;

    [SetUp]
    public void SetUp()
    {
        _brands = new();
        for (var i = 1; i <= 24; i++)
        {
            var brand = new Brand
            {
                Id = Guid.NewGuid(),
                Name = $"Brand {i}",
                Notes = "Blah, blah, blah {i}"
            };

            _brands.Add(brand);
        }

        _pageList = new PagedList<Brand>(_brands, 25, 1, 80);
    }

    [Test]
    public async Task TotalPages_Is_Calculated_Correctly()
    {
        Assert.That(_pageList.TotalPages, Is.EqualTo(4));
        Assert.That(_pageList.Count, Is.EqualTo(24));
        Assert.That(_pageList.Page, Is.EqualTo(1));
        Assert.That(_pageList.TotalCount, Is.EqualTo(80));
    }

    [Test]
    public async Task Contains_Returns_True_If_Contained()
    {
        var brand = _brands[4];
        var result = _pageList.Contains(brand);

        Assert.That(result, Is.True);
    }

    [Test]
    public async Task IndexOf_Returns_Correct_Index()
    {
        var brand = _brands[4];
        var result = _pageList.IndexOf(brand);

        Assert.That(result, Is.EqualTo(4));
    }
}
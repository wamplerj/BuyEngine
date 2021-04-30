using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace BuyEngine.Data.Sql.Tests.Integration
{
    public class ProductRepositoryTests
    {
        private const string ConnectionString = @"Data Source=NEW-PC;Initial Catalog=BuyEngine;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        [Test]
        public async Task Get_Product_By_Id()
        {

            var configuration = new Mock<IConfiguration>();
            configuration
                .Setup(c => c.GetSection("ConnectionStrings")[It.IsAny<string>()])
                .Returns(ConnectionString);

            var repository = new ProductRepository(configuration.Object);
            var result = await repository.GetAsync(1);

            Assert.That(result, Is.Not.Null);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("APP-IPH-001", result.Sku);
        }

        [Test]
        public async Task Get_Product_By_Sku()
        {

            var configuration = new Mock<IConfiguration>();
            configuration
                .Setup(c => c.GetSection("ConnectionStrings")[It.IsAny<string>()])
                .Returns(ConnectionString);

            var repository = new ProductRepository(configuration.Object);
            var result = await repository.GetAsync("APP-IPH-001");

            Assert.That(result, Is.Not.Null);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("APP-IPH-001", result.Sku);
        }

        [Test]
        public async Task Get_All_Products()
        {
            var configuration = new Mock<IConfiguration>();
            configuration
                .Setup(c => c.GetSection("ConnectionStrings")[It.IsAny<string>()])
                .Returns(ConnectionString);

            var repository = new ProductRepository(configuration.Object);
            var result = await repository.GetAllAsync(5, 1);

            Assert.That(result, Is.Not.Null);
            Assert.AreEqual(5, result.Count);
            Assert.AreEqual(8, result.TotalCount);
            Assert.AreEqual(9, result[0].Id);
            Assert.AreEqual(2, result[4].Id);
        }

        [Test]
        public async Task Get_All_Products_By_Brand()
        {
            var configuration = new Mock<IConfiguration>();
            configuration
                .Setup(c => c.GetSection("ConnectionStrings")[It.IsAny<string>()])
                .Returns(ConnectionString);

            var repository = new ProductRepository(configuration.Object);
            var result = await repository.GetAllByBrandAsync(1, 5, 1);

            Assert.That(result, Is.Not.Null);
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(8, result.TotalCount);
            Assert.AreEqual(9, result[0].Id);
            Assert.AreEqual(2, result[2].Id);
        }

        [Test]
        public async Task Get_All_Products_By_Supplier()
        {
            var configuration = new Mock<IConfiguration>();
            configuration
                .Setup(c => c.GetSection("ConnectionStrings")[It.IsAny<string>()])
                .Returns(ConnectionString);

            var repository = new ProductRepository(configuration.Object);
            var result = await repository.GetAllBySupplierAsync(1, 5, 1);

            Assert.That(result, Is.Not.Null);
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(8, result.TotalCount);
            Assert.AreEqual(9, result[0].Id);
            Assert.AreEqual(2, result[2].Id);
        }
    }
}
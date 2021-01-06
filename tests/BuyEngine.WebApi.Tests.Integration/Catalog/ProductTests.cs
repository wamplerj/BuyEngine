using NUnit.Framework;

namespace BuyEngine.WebApi.Tests.Integration.Catalog
{
    public class ProductTests
    {
        private readonly string _appBaseUrl = TestContext.Parameters["appBaseUrl"];
        private readonly string _connectionString = TestContext.Parameters["connectionString"];
        
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void An_InventoryManager_Can_Add_A_New_Product()
        {
            Assert.Fail();
        }

        [Test]
        public void An_InventoryManager_Can_Update_A_Product()
        {
            Assert.Fail();
        }

        [Test]
        public void An_InventoryManager_Can_View_A_Product()
        {
            Assert.Fail();
        }

        [Test]
        public void An_InventoryManager_Can_Disable_A_Product()
        {
            Assert.Fail();
        }

        [Test]
        public void An_InventoryManager_Can_Delete_A_Product()
        {
            Assert.Fail();
        }



    }
}
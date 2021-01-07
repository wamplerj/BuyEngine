using NUnit.Framework;
using RestSharp;
using System.Text.Json;
using System.Threading.Tasks;

namespace BuyEngine.WebApi.Tests.Integration.Catalog
{
    public class ProductTests
    {
        private string _appBaseUrl;
        private string _connectionString;
        private RestClient _client;

        [SetUp]
        public void Setup()
        {
            _appBaseUrl = TestContext.Parameters["apiBaseUrl"];
            _connectionString = TestContext.Parameters["connectionString"];

            _client = new RestClient(_appBaseUrl);
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
        public async Task An_InventoryManager_Can_View_A_Product()
        {
            var request = new RestRequest("product/1", DataFormat.Json);
            var response = await _client.ExecuteGetAsync(request);

            Assert.That(response.IsSuccessful);

            var json = JsonDocument.Parse(response.Content);
            Assert.That(json, Is.Not.Null);
            Assert.That(json.RootElement.GetProperty("id").GetInt32(), Is.EqualTo(1));
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
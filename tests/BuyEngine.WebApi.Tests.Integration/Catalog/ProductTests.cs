using Dapper;
using NUnit.Framework;
using RestSharp;
using System.Data.SqlClient;
using System.Net;
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
        public async Task An_InventoryManager_Can_Add_A_New_Product()
        {
            var productJson = @"{
	                             ""Sku"": ""INT-123"",
                                 ""Name"": ""Test Product Add"",
                                 ""Quantity"": 25,
                                 ""Price"": 49.95,
                                 ""BrandId"": 1,
                                 ""SupplierId"": 1,
                                }";

            var request = new RestRequest("product", DataFormat.Json);
            request.AddJsonBody(productJson);

            var response = await _client.ExecutePostAsync(request);
           
            Assert.That(response.IsSuccessful);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            
            var json = JsonDocument.Parse(response.Content);
            Assert.That(json, Is.Not.Null);

            var productId = json.RootElement.GetProperty("id").GetInt32();

            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var sqlStatement = "DELETE BuyEngine.Products WHERE Id = @Id";
            var rows = await connection.ExecuteAsync(sqlStatement, new { Id = productId });
            
            Assert.That(rows, Is.EqualTo(1));
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
    }
}
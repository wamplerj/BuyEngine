using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using NUnit.Framework;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace BuyEngine.Data.Sql.Tests.Integration
{
    [SetUpFixture]
    public class TestingDatabaseManager
    {
        private const string ConnectionString = "";
        private readonly string _executionFolder = Assembly.GetExecutingAssembly().Location;

        [OneTimeSetUp]
        public async Task SeedData()
        {
            await using var connection = new SqlConnection(ConnectionString);
            var server = new Server(new ServerConnection(connection));

            var sql = await File.ReadAllTextAsync($"{_executionFolder}seeddata.sql");
            server.ConnectionContext.ExecuteNonQuery(sql);

            //Verify Database is in Clean state

            //If not, clean data

            //TODO Run sql script to Seed Data
        }

        [OneTimeTearDown]
        public async Task ClearData()
        {
            //Reset database data
        }

    }
}

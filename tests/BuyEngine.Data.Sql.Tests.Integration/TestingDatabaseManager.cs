using System.Threading.Tasks;
using NUnit.Framework;

namespace BuyEngine.Data.Sql.Tests.Integration
{
    [SetUpFixture]
    public class TestingDatabaseManager
    {

        [OneTimeSetUp]
        public async Task SeedData()
        {
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

namespace BuyEngine.Catalog
{
    //TODO Make Configurable
    public static class CatalogConfiguration
    {
        public const int DefaultRecordsPerPage = 25;
        public const int CartExpirationInMinutes = 120;
        public static string DbSchema { get; set; } = "BE";
    }
}
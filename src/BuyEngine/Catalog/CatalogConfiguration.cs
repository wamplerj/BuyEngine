namespace BuyEngine.Catalog
{
    //TODO Make Configurable
    public static class CatalogConfiguration
    {
        public static string DbSchema { get; set; } = "BE";
        public const int DefaultRecordsPerPage = 25;
        public const int CartExpirationInMinutes = 120;
    }
}

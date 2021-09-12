namespace BuyEngine.Data.Sql
{
    internal static class ProductQueries
    {
        public const string GetById = @"SELECT p.Id, p.Sku, p.Name, p.Description, p.Price, p.Quantity, p.Enabled,
	                                            b.Id as BrandId, b.Name As Name, b.Notes, b.WebsiteUrl,
	                                            s.Id as SupplierId, s.Name As Name, s.Notes, s.WebsiteUrl
                                        FROM BuyEngine.Products p
	                                        INNER JOIN BuyEngine.Brands b
		                                        ON p.BrandId = b.Id
	                                        INNER JOIN BuyEngine.Suppliers s
		                                        ON p.SupplierId = s.Id
                                        WHERE p.Id = @productId";

        public const string GetBySku = @"SELECT p.Id, p.Sku, p.Name, p.Description, p.Price, p.Quantity, p.Enabled,
	                                        b.Id as BrandId, b.Name As Name, b.Notes, b.WebsiteUrl,
	                                        s.Id as SupplierId, s.Name As Name, s.Notes, s.WebsiteUrl
                                         FROM BuyEngine.Products p
	                                         INNER JOIN BuyEngine.Brands b
		                                         ON p.BrandId = b.Id
	                                         INNER JOIN BuyEngine.Suppliers s
		                                         ON p.SupplierId = s.Id
                                         WHERE p.Sku = @sku";

        public const string GetAll = @"SELECT p.Id, p.Sku, p.Name, p.Description, p.Price, p.Quantity, p.Enabled,
	                                    b.Id as BrandId, b.Name As Name, b.Notes, b.WebsiteUrl,
	                                    s.Id as SupplierId, s.Name As Name, s.Notes, s.WebsiteUrl
                                     FROM BuyEngine.Products p
	                                     INNER JOIN BuyEngine.Brands b
		                                     ON p.BrandId = b.Id
	                                     INNER JOIN BuyEngine.Suppliers s
		                                     ON p.SupplierId = s.Id
								     ORDER BY p.Name
								     OFFSET @skip ROWS
								     FETCH NEXT @pageSize ROWS ONLY;
SELECT Count(*) As TotalCount, @skip As Skip, @pageSize As PageSize FROM BuyEngine.Products p
	                                 INNER JOIN BuyEngine.Brands b
		                                 ON p.BrandId = b.Id
	                                 INNER JOIN BuyEngine.Suppliers s
		                                 ON p.SupplierId = s.Id";

        public const string GetAllBySupplierId = @"SELECT p.Id, p.Sku, p.Name, p.Description, p.Price, p.Quantity, p.Enabled,
	                                                b.Id as BrandId, b.Name As Name, b.Notes, b.WebsiteUrl,
	                                                s.Id as SupplierId, s.Name As Name, s.Notes, s.WebsiteUrl
                                                 FROM BuyEngine.Products p
	                                                 INNER JOIN BuyEngine.Brands b
		                                                 ON p.BrandId = b.Id
	                                                 INNER JOIN BuyEngine.Suppliers s
		                                                 ON p.SupplierId = s.Id
                                                 WHERE SupplierId = @supplierId
								                 ORDER BY p.Name
								                 OFFSET @skip ROWS
								                 FETCH NEXT @pageSize ROWS ONLY;
SELECT Count(*) As TotalCount, @skip As Skip, @pageSize As PageSize FROM BuyEngine.Products p
	                                 INNER JOIN BuyEngine.Brands b
		                                 ON p.BrandId = b.Id
	                                 INNER JOIN BuyEngine.Suppliers s
		                                 ON p.SupplierId = s.Id";

        public const string GetAllByBrandId = @"SELECT p.Id, p.Sku, p.Name, p.Description, p.Price, p.Quantity, p.Enabled,
	                                                b.Id as BrandId, b.Name As Name, b.Notes, b.WebsiteUrl,
	                                                s.Id as SupplierId, s.Name As Name, s.Notes, s.WebsiteUrl
                                                 FROM BuyEngine.Products p
	                                                 INNER JOIN BuyEngine.Brands b
		                                                 ON p.BrandId = b.Id
	                                                 INNER JOIN BuyEngine.Suppliers s
		                                                 ON p.SupplierId = s.Id
                                                 WHERE BrandId = @BrandId
								                 ORDER BY p.Name
								                 OFFSET @skip ROWS
								                 FETCH NEXT @pageSize ROWS ONLY;
SELECT Count(*) As TotalCount, @skip As Skip, @pageSize As PageSize FROM BuyEngine.Products p
	                                 INNER JOIN BuyEngine.Brands b
		                                 ON p.BrandId = b.Id
	                                 INNER JOIN BuyEngine.Suppliers s
		                                 ON p.SupplierId = s.Id";

        public const string GetPageInfo = @";
                                     SELECT Count(*) As TotalCount, @skip As Page, @pageSize As PageSize FROM BuyEngine.Products p
	                                 INNER JOIN BuyEngine.Brands b
		                                 ON p.BrandId = b.Id
	                                 INNER JOIN BuyEngine.Suppliers s
		                                 ON p.SupplierId = s.Id";
    }
}

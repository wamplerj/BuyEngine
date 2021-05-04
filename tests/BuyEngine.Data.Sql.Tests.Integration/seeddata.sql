DECLARE nvarchar(200) @dbName
SELECT @dbName = 'BuyEngine-IntegrationTest';

IF(DB_ID(@dbName) IS NOT NULL)
	PRINT("Dropping Integration Database");
	DROP DATABASE 

END
GO


IF(EXISTS(SELECT 1 FROM BuyEngine.CartItems))
BEGIN
	
END
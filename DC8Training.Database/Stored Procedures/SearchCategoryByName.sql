CREATE PROCEDURE [dbo].[SearchCategoryByName]
    @searchValue nvarchar(30)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @safeSearchValue nvarchar(30);
    SET @safeSearchValue = '%' + @searchValue + '%';

    DECLARE @sql nvarchar(max);
    SET @sql = N'SELECT Id, Name
                 FROM MedicineCategory
                 WHERE Name LIKE @searchValue';

    EXEC sp_executesql @sql, N'@searchValue nvarchar(30)', @searchValue = @safeSearchValue;
END;

CREATE PROCEDURE LoadDataByColumn @TableName varchar(50), @ColumnName varchar(50), @value varchar(50)
AS
BEGIN
		DECLARE @query nvarchar(1000)
		DECLARE @params nvarchar(200);

		SET	@query = 'SELECT * FROM '+QUOTENAME(@TableName)+' WHERE '+QUOTENAME(@ColumnName)+'=@value';
		SET @params = '@value varchar(50)';
    
		EXEC sp_executesql @query, @params, @value;

END
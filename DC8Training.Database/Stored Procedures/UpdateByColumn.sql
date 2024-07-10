CREATE PROCEDURE UpdateColumn 
    @TableName varchar(50),
    @ColumnName varchar(50),
    @Id varchar(50),
    @Value varchar(50),
    @message nvarchar(255) OUTPUT
AS
BEGIN

    BEGIN TRY
        DECLARE @sql nvarchar(max);
        DECLARE @params nvarchar(200);

        SET @sql = N'UPDATE ' + QUOTENAME(@TableName) + ' SET ' + QUOTENAME(@ColumnName) + ' = @Value WHERE Id = @Id';
        SET @params = N'@Value varchar(50), @Id varchar(50)';

        EXEC sp_executesql @sql, @params, @Value, @Id;

        IF @@ROWCOUNT = 0
        BEGIN
            SET @message = 'Update failed: Nothing change. '+@sql;
            RETURN @@ROWCOUNT;
        END

        ELSE
        BEGIN
            SET @message = 'Update successful. '+@sql;
            RETURN @@ROWCOUNT;
        END

    END TRY
    BEGIN CATCH
        SET @message = ERROR_MESSAGE();
    END CATCH;
END;

CREATE FUNCTION dbo.IsOnlyWhiteSpace(@input nvarchar(255))
RETURNS bit
AS
BEGIN
    DECLARE @isValid bit = 0;

    IF LTRIM(RTRIM(@input)) = ''
    BEGIN
        SET @isValid = 1; --white space
    END
    ELSE
    BEGIN
        SET @isValid = 0; 
    END

    RETURN @isValid;
END;

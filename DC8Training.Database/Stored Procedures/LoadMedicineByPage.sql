CREATE PROCEDURE [dbo].[LoadMedicineByPage]
	--direction 0 = ASC, 1 = DESC--
	@page INT = 0,
	@size INT = 1,
	@direction INT = 0
AS
BEGIN
	DECLARE @offset INT
	SET @offset = (@page)*@size
	IF @direction = 0
	BEGIN 
		SELECT * FROM [dbo].[Medicine]
		ORDER BY Id ASC
		OFFSET @offset ROWS
		FETCH NEXT @size ROWS ONLY
	END
	ELSE IF @direction = 1
	BEGIN 
		SELECT * FROM [dbo].[Medicine]
		ORDER BY Id DESC
		OFFSET @offset ROWS
		FETCH NEXT @size ROWS ONLY
	END
	ELSE
	BEGIN
		RAISERROR(N'Invalid direction!',16,1)
	END
END
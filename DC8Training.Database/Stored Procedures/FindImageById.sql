CREATE PROCEDURE [dbo].[FindImageById]
	@id int
AS
BEGIN
	SELECT * FROM [dbo].[Image]
	where [dbo].[Image].Id = @id
END
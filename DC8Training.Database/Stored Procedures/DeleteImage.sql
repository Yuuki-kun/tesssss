CREATE PROCEDURE [dbo].[DeleteImage]
	@id int
AS
	DELETE FROM Image WHERE Id = @id;
RETURN @@ROWCOUNT
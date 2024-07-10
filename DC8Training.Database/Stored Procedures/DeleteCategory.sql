CREATE PROCEDURE [dbo].[DeleteCategory]
	@id int
AS
	DELETE FROM MedicineCategory WHERE Id = @id;
RETURN @@ROWCOUNT

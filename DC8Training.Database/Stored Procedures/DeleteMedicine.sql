CREATE PROCEDURE [dbo].[DeleteMedicine]
	@id int
AS
	DELETE FROM Image WHERE MedicineId = @id;
	DELETE FROM Medicine WHERE Id = @id;
RETURN @@ROWCOUNT
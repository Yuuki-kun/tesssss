CREATE PROCEDURE [dbo].[UpdateImage]
	@id int, 
	@name nvarchar(30),
	@url nvarchar(1000),
	@medicineID int = NULL
AS
	BEGIN
		UPDATE Image 
		SET 
		Name = @name,
		URL = @url,
		MedicineId = @medicineID
		WHERE Id = @id;
	
	RETURN @@ROWCOUNT;
END

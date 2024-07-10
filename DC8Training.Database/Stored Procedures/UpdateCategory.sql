CREATE PROCEDURE [dbo].[UpdateCategory]
	@id int,
	@value nvarchar(255)
AS
	BEGIN
		UPDATE MedicineCategory 
		SET 
		Name = @value where Id = id; 
	RETURN @@ROWCOUNT;
END

CREATE PROCEDURE [dbo].[FindCategoryById]
	@id int
AS
BEGIN
	SELECT * FROM [dbo].[MedicineCategory]
	where [dbo].[MedicineCategory].Id = @id
END
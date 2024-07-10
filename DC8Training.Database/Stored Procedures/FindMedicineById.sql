CREATE PROCEDURE [dbo].[FindMedicineById]
	@id int
AS
BEGIN
	SELECT *, [dbo].MedicineCategory.Name as CategoryName FROM [dbo].[Medicine]
	LEFT JOIN [dbo].[MedicineCategory] ON [dbo].[Medicine].CategoryId = [dbo].[MedicineCategory].Id
	where [dbo].[Medicine].Id = @id
END
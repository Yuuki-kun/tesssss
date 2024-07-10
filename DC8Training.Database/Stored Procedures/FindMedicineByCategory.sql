CREATE PROCEDURE [dbo].[FindMedicineByCategory]
	@categoryId int
AS
BEGIN
	SELECT Id, Name, Description, Price, PrimaryImageId, PopularityMedicine FROM [dbo].[Medicine] 
	WHERE CategoryId=@categoryId
END
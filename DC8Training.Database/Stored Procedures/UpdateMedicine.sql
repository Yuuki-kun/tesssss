CREATE PROCEDURE [dbo].[UpdateMedicine]
	@id int,
	@name nvarchar(30),
	@description nvarchar(255) = 'Product descriptions',
	@price decimal(19, 4),
	@primaryImageId int = 0,
	@popularityMedicine bit = 0,
	@categoryId int=null
AS
	BEGIN 
		UPDATE Medicine
		SET
		Name = @name,
		Description = @description,
		Price = @price,
		PrimaryImageId = @primaryImageId,
		PopularityMedicine = @popularityMedicine,
		CategoryId = @categoryId
		WHERE Id = @id; 

	RETURN @@ROWCOUNT;
END
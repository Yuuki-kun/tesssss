CREATE PROCEDURE [dbo].[AddMedicine]
	@name nvarchar(30),
	@description nvarchar(255) = 'Product descriptions',
	@price decimal(19, 4),
	@primaryImageId int = 0,
	@popularityMedicine bit = 0,
	@categoryId int=null,

	@message nvarchar(4000) output,
	@medicineId int output
AS

/*
	@Description: This procedure is used to add a new medicine to the database.
	@Param:
	@Output:
		- @message: Describes the state after execution.
		- @medicineId: Returns Medicine's ID if successful otherwise returns -1
*/

BEGIN
	
	SET NOCOUNT ON;

	--check null name & empty name, price

	IF @name IS NULL OR @name='' OR dbo.IsOnlyWhiteSpace(@name)=1 OR @price IS NULL 
	BEGIN
		SET @message = 'ADD MEDICINE FAILED! Some parameters are NULL or EMPTY.';
		SET @medicineId = -1;
		RETURN;
	END

	--make sure price is valid
	IF @price < 0 
	BEGIN 
		
        SET @message = 'ADD MEDICINE FAILED! Price cannot be negative.';
		SET @medicineId = -1;
		RETURN;
	END

	--main process
	BEGIN TRY
		INSERT INTO [dbo].[Medicine] (Name, Description, Price, PrimaryImageId, PopularityMedicine, CategoryId) 
		VALUES (@name, @description, @price, @primaryImageId, @popularityMedicine, @categoryId)
		
		SET @message = 'SUCCESS';
		SET @medicineId = SCOPE_IDENTITY();

	END TRY

	BEGIN CATCH
		SET @message = 'ADD MEDICINE FAILED! ' + ERROR_MESSAGE();
		SET @medicineId = -1;
	END CATCH
END
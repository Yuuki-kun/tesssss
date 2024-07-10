CREATE PROCEDURE [dbo].[AddMedicineCategory]
	@name nvarchar(30),
	@message nvarchar(4000) output,
	@categoryId int output
AS
BEGIN
	
	IF @name IS NULL OR dbo.IsOnlyWhiteSpace(@name)=1
	BEGIN
		SET @message = 'ADD CATEGORY FAILED! Some parameters are NULL or EMPTY.';
		SET @categoryId = -1;
		RETURN
	END

	BEGIN TRY
		SET NOCOUNT ON 
		INSERT INTO [dbo].[MedicineCategory] (Name) 
		VALUES (@name)

		SET @message = 'SUCCESS';
		SET @categoryId = SCOPE_IDENTITY();
	END TRY
	BEGIN CATCH
		SET @message = 'ADD CATEGORY FAILD! ' + ERROR_MESSAGE();
		SET @categoryId = -1;
	END CATCH
END
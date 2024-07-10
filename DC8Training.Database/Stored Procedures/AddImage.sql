CREATE PROCEDURE [dbo].[AddImage]
	@name nvarchar(1000),
	@url nvarchar(1000),
	@medicineID int = NULL,
	@message nvarchar(4000) output,
	@imageId int output
AS
BEGIN
	BEGIN TRY
		SET NOCOUNT ON 
		INSERT INTO [dbo].[Image] (Name, URL, MedicineId) 
		VALUES (@name, @url, @medicineID)
		SET @message = 'SUCCESS';
		SET @imageId = SCOPE_IDENTITY();
	END TRY
	BEGIN CATCH
		SET @message = 'ADD IMAGE FAILD! ' + ERROR_MESSAGE();
		SET @imageId = -1;
	END CATCH
END

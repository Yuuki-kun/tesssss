CREATE PROCEDURE [dbo].[AddMedicineToCategory]
    @categoryId int,
    @medicineId int,
    @message nvarchar(255) OUTPUT
AS

/*
	Add Medicine to Category by Update CategoryId column in Table Medicine
*/

BEGIN

    SET NOCOUNT ON;

    BEGIN TRY

        UPDATE [dbo].[Medicine]
        SET CategoryId = @categoryId
        WHERE Id = @medicineId;

        IF @@ROWCOUNT = 0
        BEGIN
            SET @message = 'Update failed: Medicine not found or already in the specified category.';
        END

        ELSE
        BEGIN
            SET @message = 'Update successful: Medicine category updated.';
        END
    END TRY

    BEGIN CATCH
        SET @message = 'Update failed: ' + ERROR_MESSAGE();
    END CATCH

    RETURN @@ROWCOUNT;

END;

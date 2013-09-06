CREATE PROCEDURE [dbo].[GetTransactionCompleted]
	@ExternalId	NVARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TOP 1
		ISNULL(IsCompleted, 0)
	FROM dbo.Donation 
	WHERE ExternalId = @ExternalId;
END;

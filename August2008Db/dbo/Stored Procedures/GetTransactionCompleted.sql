CREATE PROCEDURE [dbo].[GetTransactionCompleted]
	@ExternalId	NVARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TOP 1
		CAST(ISNULL(IsCompleted, 0) AS BIT)
	FROM dbo.Donation 
	WHERE ExternalId = @ExternalId;
END;

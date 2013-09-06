CREATE PROCEDURE [dbo].[UpdateDonationStatus]
	@ExternalId				NVARCHAR(50),
	@ExternalStatus			NVARCHAR(25)	= NULL,
	@IsCompleted			BIT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE dbo.Donation 
	SET
		IsCompleted		= @IsCompleted,
		ExternalStatus	= @ExternalStatus
	WHERE 
		@ExternalId		= @ExternalId;		
END;


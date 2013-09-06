CREATE PROCEDURE [dbo].[CompleteDonation]
	@ExternalId				NVARCHAR(50),
	@ExternalStatus			NVARCHAR(25)	= NULL,
	@IsCompleted			BIT,
	@CityId					INT				= NULL,
	@StateId				INT				= NULL,
	@CountryId				INT				= NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE dbo.Donation 
	SET
		IsCompleted		= @IsCompleted,
		ExternalStatus	= @ExternalStatus,
		CountryId		= @CountryId,
		CityId			= @CityId,
		StateId			= @StateId
	WHERE 
		ExternalId		= @ExternalId;		
END;


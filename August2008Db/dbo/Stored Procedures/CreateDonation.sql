CREATE PROCEDURE [dbo].[CreateDonation]
	@DonationProviderId		INT,
	@UserId					INT,
	@ExternalId				NVARCHAR(50),
	@ExternalStatus			NVARCHAR(25)	= NULL,
	@IsCompleted			BIT,
	@Amount					MONEY,
	@Currency				NVARCHAR(10)	= NULL,
	@UserMessage			NVARCHAR(500)	= NULL,
	@ProviderData			XML				= NULL,
	@CityId					INT				= NULL,
	@StateId				INT				= NULL,
	@CountryId				INT				= NULL,
	@DonationId				INT				= NULL OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	IF EXISTS(SELECT TOP 1 1 FROM dbo.Donation WITH (NOLOCK) WHERE ExternalId = @ExternalId)
	BEGIN
		SELECT 
			@DonationId
		FROM dbo.Donation WITH (NOLOCK)
		WHERE ExternalId = @ExternalId;
	END
	ELSE
	BEGIN

		INSERT INTO dbo.Donation (
			 DonationProviderId
			,UserId
			,ExternalId
			,ExternalStatus
			,IsCompleted
			,Amount
			,Currency
			,UserMessage
			,ProviderData
			,CityId
			,StateId
			,CountryId
		)
		VALUES (
			 @DonationProviderId
			,@UserId
			,@ExternalId
			,@ExternalStatus
			,@IsCompleted
			,@Amount
			,@Currency
			,@UserMessage
			,@ProviderData
			,@CityId
			,@StateId
			,@CountryId
		 )
		SET @DonationId = SCOPE_IDENTITY();
	END;
END;
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
	@Latitude				FLOAT			= NULL,
	@Longitude				FLOAT			= NULL,
	@DonationId				INT				= NULL OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @point GEOGRAPHY;

	IF @Latitude IS NULL OR @Longitude IS NULL 
	BEGIN
		SET @point = 'POINT EMPTY';
		SET @point.STSrid = 4326;
	END;

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
		,Geo
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
		,@point
	 )
	SET @DonationId = SCOPE_IDENTITY();
END;
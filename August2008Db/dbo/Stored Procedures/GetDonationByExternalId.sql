CREATE PROCEDURE [dbo].[GetDonationByExternalId]
	@ExternalId				NVARCHAR(50),
	@DonationId				INT				= NULL OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TOP 1
		 DonationId
		,DonationProviderId
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
	FROM dbo.Donation WITH (NOLOCK)
	WHERE ExternalId = @ExternalId;
END;

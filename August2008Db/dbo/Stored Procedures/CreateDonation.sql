CREATE PROCEDURE [dbo].[CreateDonation]
	@DonationProviderId		INT,
	@UserId					INT,
	@Amount					MONEY,
	@Currency				NVARCHAR(10) = NULL,
	@UserMessage			NVARCHAR(500)= NULL,
	@ProviderData			XML			 = NULL,
	@DonationId				INT			 = NULL OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO dbo.Donation (
		 DonationProviderId
		,UserId
		,Amount
		,Currency
		,UserMessage
		,ProviderData
	)
	VALUES (
		 @DonationProviderId
		,@UserId
		,@Amount
		,@Currency
		,@UserMessage
		,@ProviderData
	 )
	SET @DonationId = SCOPE_IDENTITY();
END;
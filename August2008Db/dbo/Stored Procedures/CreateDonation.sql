CREATE PROCEDURE [dbo].[CreateDonation]
	@DonationProviderId		INT,
	@UserId					INT,
	@FirstName				NVARCHAR(50) = NULL,
	@LastName				NVARCHAR(50) = NULL,
	@Amount					MONEY,
	@Currency				NVARCHAR(10) = NULL,
	@UserMessage			NVARCHAR(500)= NULL,
	@ProviderData			XML			 = NULL,
	@Email					NVARCHAR(50) = NULL,
	@DonationId				INT			 = NULL OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO dbo.Donation (
		 DonationProviderId
		,UserId
		,FirstName
		,LastName
		,Amount
		,Currency
		,UserMessage
		,ProviderData
		,Email
	)
	VALUES (
		 @DonationProviderId
		,@UserId
		,@FirstName
		,@LastName
		,@Amount
		,@Currency
		,@UserMessage
		,@ProviderData
		,@Email	
	 )
	SET @DonationId = SCOPE_IDENTITY();
END;
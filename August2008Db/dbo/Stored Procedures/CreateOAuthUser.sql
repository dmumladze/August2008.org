CREATE PROCEDURE [dbo].[CreateOAuthUser]
	@UserId			INT = NULL,
	@Email			NVARCHAR(50),
	@ProviderId		NVARCHAR(250),
    @ProviderName	NVARCHAR(50),      
    @ProviderData	XML,
	@OAuthUserId	INT = NULL OUTPUT	
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO dbo.OAuthUser (
		UserId,
		Email,
		ProviderId,
		ProviderName,
		ProviderData
	)
	VALUES (
		@UserId,
		@Email,
		@ProviderId,
		@ProviderName,
		@ProviderData
	);
	SELECT @OAuthUserId = SCOPE_IDENTITY();
END;
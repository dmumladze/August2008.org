CREATE PROCEDURE [dbo].[CreateOAuthUser]
	@UserId			INT,
	@ProviderId		NVARCHAR(250),
    @ProviderName	NVARCHAR(50),      
    @ProviderData	XML,
	@OAuthUserId	INT = NULL OUTPUT	
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO dbo.OAuthUser (
		UserId,
		ProviderId,
		ProviderName,
		ProviderData
	)
	VALUES (
		@UserId,
		@ProviderId,
		@ProviderName,
		@ProviderData
	);
	SELECT @OAuthUserId = SCOPE_IDENTITY();
	
END;
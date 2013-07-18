CREATE PROCEDURE [dbo].[CreateOAuthUser]
	@UserId			INT = NULL,
	@ProviderId		NVARCHAR(250),
    @ProviderName	NVARCHAR(50),      
    @ProviderData	XML,
	@OAuthUserId	INT = NULL OUTPUT	
AS
BEGIN
	SET NOCOUNT ON;

	IF (EXISTS(SELECT 1  FROM dbo.OAuthUser (NOLOCK) WHERE ProviderId = @ProviderId))
	BEGIN
		IF (@UserId IS NOT NULL)
		BEGIN
			UPDATE dbo.OAuthUser
			SET
				UserId = @UserId
			WHERE ProviderId = @ProviderId;
		END;
		SELECT 
			@OAuthUserId = OAuthUserId 
		FROM dbo.OAuthUser (NOLOCK) 
		WHERE ProviderId = @ProviderId
	END
	ELSE
	BEGIN
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
END;
CREATE PROCEDURE [dbo].[GetUserRegistered]
	@Email			NVARCHAR(50),
	@Provider		NVARCHAR(50),
	@UserId			INT	= NULL	OUTPUT,
	@IsOAuthUser	INT	= 0		OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @Provider2 VARCHAR(50);

	SELECT TOP 1
		@UserId	= UserId
	FROM dbo.[User] WITH(NOLOCK)
	WHERE Email = @Email; 

	IF @UserId IS NOT NULL
	BEGIN
		IF EXISTS(SELECT TOP 1 1 FROM dbo.OAuthUser WITH(NOLOCK) WHERE UserId = @UserId AND ProviderName = @Provider)
			SET @IsOAuthUser = 1;
		ELSE
			SET @IsOAuthUser = 0;
	END;
END;
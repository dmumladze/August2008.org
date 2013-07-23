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
		@UserId		= u.UserId,
		@Provider2	= oa.ProviderName
	FROM dbo.OAuthUser oa (NOLOCK)
	INNER JOIN dbo.[User] u (NOLOCK) ON oa.UserId = u.UserId
	WHERE oa.Email = @Email; 

	IF @Provider2 = @Provider 
		SET @IsOAuthUser = 1;
	ELSE
		SET @IsOAuthUser = 0;
END;
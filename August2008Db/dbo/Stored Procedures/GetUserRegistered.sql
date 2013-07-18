CREATE PROCEDURE [dbo].[GetUserRegistered]
	@ProviderId		NVARCHAR(250),
	@UserId			INT	= NULL	OUTPUT,
	@IsRegistered	BIT = 0		OUTPUT,
	@IsOAuthUser	BIT = 0		OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TOP 1
		@UserId			= u.UserId,
		@IsRegistered	= CASE WHEN u.Password IS NULL THEN 0 ELSE 1 END
	FROM dbo.OAuthUser oa (NOLOCK)
	LEFT JOIN dbo.[User] u (NOLOCK) ON oa.UserId = u.UserId
	WHERE oa.ProviderId = @ProviderId

	SET @IsOAuthUser = @@ROWCOUNT;
END;

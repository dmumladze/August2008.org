CREATE PROCEDURE [dbo].[GetUserRegistered]
	@Email			NVARCHAR(50),
	@UserId			INT	= NULL	OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TOP 1
		@UserId			= u.UserId
	FROM dbo.OAuthUser oa (NOLOCK)
	INNER JOIN dbo.[User] u (NOLOCK) ON oa.UserId = u.UserId
	WHERE oa.Email = @Email; 
END;

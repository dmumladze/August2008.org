CREATE PROCEDURE [dbo].[GetUserRoles]
	@UserId	INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		r.RoleId,
		r.Name
	FROM dbo.[Role] r (NOLOCK)
	INNER JOIN dbo.UserRole ur (NOLOCK) ON r.RoleId =ur.RoleId 
	WHERE ur.UserId = @UserId;
END;

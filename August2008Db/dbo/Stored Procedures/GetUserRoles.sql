CREATE PROCEDURE [dbo].[GetUserRoles] 
	@UserId	INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		r.RoleId,
		r.Name,
		ur.UserId
	FROM dbo.[Role] r (NOLOCK)
	LEFT JOIN dbo.UserRole ur (NOLOCK) ON r.RoleId = ur.RoleId AND ur.UserId = @UserId;
END;

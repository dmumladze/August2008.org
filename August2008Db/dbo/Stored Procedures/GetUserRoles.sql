CREATE PROCEDURE [dbo].[GetUserRoles] 
	@UserId	INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		r.RoleId,
		r.Name,
		ur.UserId
	FROM dbo.[Role] r WITH (NOLOCK)
	LEFT JOIN dbo.UserRole ur WITH (NOLOCK) ON r.RoleId = ur.RoleId AND ur.UserId = @UserId;
END;

CREATE PROCEDURE [dbo].[RevokeUserFromRole]
	@UserId		INT,
	@RoleId		INT
AS
BEGIN
	SET NOCOUNT ON;

	IF (EXISTS(SELECT 1 
			FROM dbo.UserRole WITH (NOLOCK) 
			WHERE UserId = @UserId AND RoleId = @RoleId))
	BEGIN
		DELETE FROM dbo.UserRole
		WHERE UserId = @UserId AND RoleId = @RoleId;
	END;
END;

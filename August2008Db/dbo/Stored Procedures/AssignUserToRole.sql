CREATE PROCEDURE [dbo].[AssignUserToRole]
	@UserId		INT,
	@RoleId		INT
AS
BEGIN
	SET NOCOUNT ON;

	IF (NOT EXISTS(
				SELECT 1 
				FROM dbo.UserRole WITH (NOLOCK) 
				WHERE UserId = @UserId AND RoleId = @RoleId))
	BEGIN
		INSERT INTO dbo.UserRole VALUES(@UserId, @RoleId);
	END;
END;

CREATE PROCEDURE [dbo].[GetRoles]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		RoleId,
		Name,
		Description
	FROM [dbo].[Role]
END;


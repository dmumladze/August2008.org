CREATE PROCEDURE [dbo].[GetUsers]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
	 [UserId]
	,[Email]
	,[DisplayName]
	FROM [dbo].[User] (NOLOCK)

END
CREATE PROCEDURE [dbo].[GetUsers]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
	 [UserId]
	,[UserName]
	,[Email]
	,[DisplayName]
	FROM [dbo].[User]

END
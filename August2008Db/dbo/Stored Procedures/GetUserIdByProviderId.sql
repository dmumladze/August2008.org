CREATE PROCEDURE [dbo].[GetUserIdByProviderId]
	@ProviderId		NVARCHAR(250)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT	 UserId
	FROM dbo.OAuthUser (NOLOCK)
	WHERE ProviderId = @ProviderId;

END;

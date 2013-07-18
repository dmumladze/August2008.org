CREATE PROCEDURE [dbo].[GetUserByUserId]
	@UserId		INT
AS
BEGIN
	SET NOCOUNT ON;

	-- user 
	SELECT	 UserId
			,Email
			,DisplayName
			,MemberSince
			,SuperAdmin
	FROM dbo.[User] (NOLOCK)
	WHERE UserId = @UserId;

	-- user profile
	SELECT	 Dob
			,Nationality
	FROM dbo.UserProfile
	WHERE UserId = @UserId;

	-- language
	SELECT	 lang.LanguageId
			,lang.DisplayName
			,lang.EnglishName
			,lang.Culture
	FROM dbo.[Language] lang (NOLOCK)
	INNER JOIN dbo.UserProfile up (NOLOCK) ON lang.LanguageId = up.LanguageId AND up.UserId = @UserId;

	-- provider
	SELECT	 ProviderId
			,ProviderName
			--,ProviderData
	FROM dbo.OAuthUser (NOLOCK)
	WHERE UserId = @UserId;

	-- roles
	SELECT	 r.Name
	FROM dbo.[Role] r (NOLOCK)
	INNER JOIN dbo.UserRole ur (NOLOCK) ON r.RoleId = ur.RoleId AND ur.UserId = @UserId;

END

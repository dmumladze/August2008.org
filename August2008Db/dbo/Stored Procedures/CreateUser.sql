CREATE PROCEDURE [dbo].[CreateUser]
	@UserName		NVARCHAR(50),
	@Email			NVARCHAR(50),
	@DisplayName	NVARCHAR(50)	= NULL,
	@LanguageId		INT,
	@ProviderId		NVARCHAR(250),
    @ProviderName	NVARCHAR(50),      
    @ProviderData	XML,
	@UserId			INT				= NULL OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	-- create user
	INSERT INTO dbo.[User] (
		UserName,
		Email,		
		DisplayName		
	)
	VALUES (
		@UserName,
		@Email,
		@DisplayName		
	);

	SELECT @UserId = SCOPE_IDENTITY();

	-- populate profile
	INSERT INTO dbo.UserProfile (
		UserId,
		LanguageId
	)
	VALUES (
		@UserId,
		@LanguageId
	);

	-- populate OAuth stuff
	INSERT INTO dbo.OAuthUser (
		UserId,
		ProviderId,
		ProviderName,
		ProviderData
	)
	VALUES (
		@UserId,
		@ProviderId,
		@ProviderName,
		@ProviderData
	);
END

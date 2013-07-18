CREATE PROCEDURE [dbo].[CreateUserProfile]
	@UserId			INT,
	@LanguageId		INT,
	@Dob			DATETIME	= NULL,
	@Nationality	NVARCHAR(50)= NULL,
	@UserProfileId	INT			= NULL OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	IF (EXISTS(SELECT 1 FROM dbo.UserProfile (NOLOCK) WHERE UserId = @UserId))
	BEGIN
		SELECT TOP 1
			@UserProfileId = UserProfileId
		FROM dbo.UserProfile (NOLOCK)
		WHERE UserId = @UserId;
	END
	ELSE
	BEGIN
		INSERT INTO dbo.UserProfile (
			UserId,
			LanguageId,
			Dob,
			Nationality
		)
		VALUES (
			@UserId,
			@LanguageId,
			@Dob,
			@Nationality
		);
		SELECT @UserProfileId = SCOPE_IDENTITY();
	END;
END;
CREATE PROCEDURE [dbo].[CreateUserProfile]
	@UserId			INT,
	@LanguageId		INT,
	@Dob			DATETIME	= NULL,
	@Nationality	NVARCHAR(50)= NULL,
	@UserProfileId	INT			= NULL OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

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
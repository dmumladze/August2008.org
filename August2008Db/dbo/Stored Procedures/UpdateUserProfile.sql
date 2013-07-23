CREATE PROC dbo.UpdateUserProfile
	@UserId			INT,
	@LanguageId		INT,
	@Dob			DATETIME	 = NULL,
	@Nationality	NVARCHAR(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE dbo.UserProfile
	SET	LanguageId = @LanguageId,
		Dob		   = @Dob,
		Nationality= @Nationality
	WHERE UserId = @UserId;
END
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
		Dob		   = ISNULL(@Dob,Dob),
		Nationality= ISNULL(@Nationality,Nationality)
	WHERE UserId = @UserId AND
		(@Dob IS NOT NULL OR
		 @Nationality IS NOT NULL);
END
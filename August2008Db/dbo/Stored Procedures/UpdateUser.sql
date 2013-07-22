CREATE PROC dbo.UpdateUser
	@UserId			INT,
	@Email			NVARCHAR(50) = NULL,
	@DisplayName	NVARCHAR(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE dbo.[User]
	SET Email	    = ISNULL(@Email, Email),
		DisplayName = ISNULL(@DisplayName, DisplayName)
	WHERE UserId = @UserId
	AND (@Email IS NOT NULL OR 
		 @DisplayName IS NOT NULL); 
END;
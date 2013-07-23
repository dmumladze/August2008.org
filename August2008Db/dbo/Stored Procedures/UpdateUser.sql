CREATE PROC dbo.UpdateUser
	@UserId			INT,
	@Email			NVARCHAR(50) = NULL,
	@DisplayName	NVARCHAR(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE dbo.[User]
	SET Email	    = @Email,
		DisplayName = @DisplayName
	WHERE UserId = @UserId; 
END;
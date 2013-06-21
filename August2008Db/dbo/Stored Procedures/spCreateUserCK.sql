CREATE PROCEDURE dbo.spCreateUserCK
	@UserId		INT = NULL,
	@UserName	NVARCHAR(50),
	@Email		NVARCHAR(50),
	@DisplayName NVARCHAR(50),
	@DateCerated DATETIME = GETDATE
AS
	DECLARE @ReturnValue	INT

IF (@UserId IS NULL)
	BEGIN
		INSERT INTO dbo.[User]
			(UserName, Email, DisplayName,DateCreated)
		VALUES(@UserName,@Email,@DisplayName,@DateCerated)
		SELECT @ReturnValue = SCOPE_IDENTITY()
	END
ELSE 
	BEGIN
		UPDATE dbo.[User]
		SET UserName = @UserName,
			Email	 = @Email,
		DisplayName	 = @DisplayName,
		DateCreated  = @DateCerated
		WHERE UserId = @UserId

	SELECT @ReturnValue = @UserId
	END

	RETURN @ReturnValue
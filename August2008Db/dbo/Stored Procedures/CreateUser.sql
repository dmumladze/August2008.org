CREATE PROCEDURE [dbo].[CreateUser]
	@Email			NVARCHAR(50)	= NULL,
	@DisplayName	NVARCHAR(50)	= NULL,
	@Password		NVARCHAR(25)	= NULL,
	@UserId			INT				= NULL OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	IF (EXISTS(SELECT 1 FROM dbo.[User] (NOLOCK) WHERE Email = @Email AND Password = @Password))
	BEGIN
		SELECT
			@UserId = UserId
		FROM dbo.[User] (NOLOCK)
		WHERE Email = @Email 
		AND Password = @Password
	END
	ELSE
	BEGIN
		INSERT INTO dbo.[User] (
			 Email
			,DisplayName
			,Password
		)
		VALUES (
			 @Email
			,@DisplayName
			,@Password
		);
		SELECT @UserId = SCOPE_IDENTITY();
	END;
END;
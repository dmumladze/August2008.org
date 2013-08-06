CREATE PROCEDURE [dbo].[CreateUser]
	@Email			NVARCHAR(50)	= NULL,
	@DisplayName	NVARCHAR(50)	= NULL,
	@UserId			INT				= NULL OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	IF (EXISTS(SELECT 1 FROM dbo.[User] WITH (NOLOCK) WHERE Email = @Email))
	BEGIN
		SELECT
			@UserId = UserId
		FROM dbo.[User] WITH (NOLOCK)
		WHERE Email = @Email 
	END
	ELSE
	BEGIN
		INSERT INTO dbo.[User] (
			 Email
			,DisplayName
		)
		VALUES (
			 @Email
			,@DisplayName
		);
		SELECT @UserId = SCOPE_IDENTITY();
	END;
END;
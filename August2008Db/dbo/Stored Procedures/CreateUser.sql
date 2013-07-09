CREATE PROCEDURE [dbo].[CreateUser]
	@Email			NVARCHAR(50)	= NULL,
	@DisplayName	NVARCHAR(50)	= NULL,
	@UserId			INT				= NULL OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
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
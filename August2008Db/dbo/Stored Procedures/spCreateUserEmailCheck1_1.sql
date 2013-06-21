 CREATE PROCEDURE dbo.spCreateUserEmailCheck1

	@UserName	NVARCHAR(50),
	@Email		NVARCHAR(50),
	@DisplayName NVARCHAR(50),
	@DateCreated DATETIME = NULL
AS
		UPDATE dbo.[User]
		SET UserName = @UserName,
			DisplayName=@DisplayName
		WHERE Email = @Email
	IF @@ROWCOUNT <> 1
BEGIN
	INSERT INTO dbo.[User]
		(UserName, Email, DisplayName,DateCreated)
	VALUES
	    (@UserName,@Email,@DisplayName,COALESCE(@DateCreated, GETDATE()))	
END
	
CREATE PROCEDURE [dbo].[SearchUsers]
	@StartsWith	NVARCHAR(50)	= NULL
AS
BEGIN
	SET NOCOUNT ON;
	SELECT TOP 100 
		 UserId
		,Email
		,DisplayName
		,MemberSince
		,SuperAdmin
	FROM [dbo].[User] WITH (NOLOCK)
	WHERE (@StartsWith IS NULL OR DisplayName LIKE @StartsWith + '%')
	ORDER BY DisplayName;
END

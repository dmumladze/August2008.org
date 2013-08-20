CREATE PROCEDURE dbo.Logger
    @Date		DATETIME,
    @Thread		VARCHAR(255),
	@Username	NVARCHAR(50)	= NULL,
    @Level		VARCHAR (50),
    @Logger		VARCHAR (255),
    @Message	VARCHAR (4000),
    @Exception	VARCHAR (2000)	= NULL
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[Log] (
		 [Date]
		,[Thread]
		,[Username]
		,[Level]
		,[Logger]
		,[Message]
		,[Exception]
	)
	VALUES (
		 @Date
		,@Thread
		,@Username
		,@Level
		,@Logger
		,@Message
		,@Exception
	);
END;

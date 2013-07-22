CREATE TABLE [dbo].[OAuthUser]
(
	[OAuthUserId]	INT IDENTITY (1, 1) NOT NULL,
    [UserId]		INT					NULL, 
    [ProviderId]	NVARCHAR(250)		NOT NULL, 
    [ProviderName]	NVARCHAR(50)		NOT NULL, 
    [ProviderData]	XML					NULL,
	[Email]			NVARCHAR(50)		NOT NULL
	CONSTRAINT [PK_OAuthUser] PRIMARY KEY ([OAuthUserId])
	CONSTRAINT [FK_OAuthUser_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User]([UserId])
)

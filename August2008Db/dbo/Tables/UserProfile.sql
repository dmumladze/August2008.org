CREATE TABLE [dbo].[UserProfile]
(
	[UserProfileId]	INT IDENTITY (1, 1) NOT NULL,
    [UserId]		INT				NOT NULL, 
    [LanguageId]	INT				NOT NULL,
	[Dob]			DATETIME		NULL,
	[Nationality]	NVARCHAR(50)	NULL
	CONSTRAINT [PK_UserProfile] PRIMARY KEY ([UserProfileId])
	CONSTRAINT [FK_UserProfile_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User]([UserId])
	CONSTRAINT [FK_UserProfile_Language] FOREIGN KEY ([LanguageId]) REFERENCES [dbo].[Language]([LanguageId])
)

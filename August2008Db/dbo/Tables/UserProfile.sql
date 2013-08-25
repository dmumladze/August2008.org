CREATE TABLE [dbo].[UserProfile]
(
	[UserProfileId]	INT IDENTITY (1, 1) NOT NULL,
    [UserId]		INT				NOT NULL, 
    [LanguageId]	INT				NOT NULL,
	[Dob]			DATETIME		NULL,
	[Nationality]	NVARCHAR(50)	NULL,
	[Street]	NVARCHAR(100)		NULL,
	[CityId]		INT				NULL,
	[StateId]		INT				NULL,
	[CountryId]		INT				NULL,
	[Geo]			GEOGRAPHY		DEFAULT('POINT EMPTY')
	CONSTRAINT [PK_UserProfile] PRIMARY KEY ([UserProfileId])
	CONSTRAINT [FK_UserProfile_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User]([UserId])
	CONSTRAINT [FK_UserProfile_Language] FOREIGN KEY ([LanguageId]) REFERENCES [dbo].[Language]([LanguageId])
	CONSTRAINT [FK_UserProfile_City] FOREIGN KEY ([CityId]) REFERENCES [dbo].[City]([CityId])
	CONSTRAINT [FK_UserProfile_State] FOREIGN KEY ([StateId]) REFERENCES [dbo].[State]([StateId])
	CONSTRAINT [FK_UserProfile_Country] FOREIGN KEY ([CountryId]) REFERENCES [dbo].[Country]([CountryId])
)

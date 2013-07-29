CREATE TABLE [dbo].[HeroTranslation] (
    [HeroId]      INT            NOT NULL,
	[MilitaryRankId]  INT        NULL,
    [MilitaryGroupId] INT        NULL,
	[MilitaryAwardId] INT		 NULL,
    [FirstName]   NVARCHAR (50)  NOT NULL,
    [LastName]    NVARCHAR (75)  NOT NULL,
	[MiddleName]  NVARCHAR (50)  NULL,
    [Biography]   NVARCHAR (MAX) NULL,
    [DateUpdated] DATETIME       NOT NULL DEFAULT GETDATE(),
    [UpdatedBy]   INT            NOT NULL,
    [LanguageId]  INT            NOT NULL,
    [Version]     ROWVERSION     NOT NULL,
    CONSTRAINT [PK_HeroTranslation] PRIMARY KEY CLUSTERED ([HeroId] ASC, [LanguageId] ASC),
	CONSTRAINT [FK_HeroTranslation_MilitaryGroup] FOREIGN KEY ([MilitaryGroupId]) REFERENCES [dbo].[MilitaryGroup] ([MilitaryGroupId]),
    CONSTRAINT [FK_HeroTranslation_MilitaryRank] FOREIGN KEY ([MilitaryRankId]) REFERENCES [dbo].[MilitaryRank] ([MilitaryRankId]),
    CONSTRAINT [FK_HeroTranslation_MilitaryAward] FOREIGN KEY (MilitaryAwardId) REFERENCES [dbo].MilitaryAward (MilitaryAwardId),
    CONSTRAINT [FK_Hero_Language] FOREIGN KEY ([LanguageId]) REFERENCES [dbo].[Language] ([LanguageId]),
    CONSTRAINT [FK_Hero_User_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[User] ([UserId]),
    CONSTRAINT [FK_HeroTranslation_Hero] FOREIGN KEY ([HeroId]) REFERENCES [dbo].[Hero] ([HeroId])
);




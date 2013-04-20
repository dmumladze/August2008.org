CREATE TABLE [dbo].[HeroTranslation] (
    [HeroId]          INT            NOT NULL,
    [FirstName]       NVARCHAR (50)  NOT NULL,
    [LastName]        NVARCHAR (75)  NOT NULL,
    [Biography]       NVARCHAR (MAX) NULL,
    [DateUpdated]     DATETIME       NOT NULL,
    [UpdatedBy]       INT            NOT NULL,
    [LanguageId]      INT            NOT NULL,
    CONSTRAINT [FK_Hero_Language] FOREIGN KEY ([LanguageId]) REFERENCES [dbo].[Language] ([LanguageId]),
	CONSTRAINT [FK_Hero_User_UpdatedBy] FOREIGN KEY ([UPdatedBy]) REFERENCES [dbo].[User] ([UserId]), 
    CONSTRAINT [PK_HeroTranslation] PRIMARY KEY ([HeroId], [LanguageId]), 
    CONSTRAINT [FK_HeroTranslation_Hero] FOREIGN KEY ([HeroId]) REFERENCES [dbo].[Hero]([HeroId]), 
);


CREATE TABLE [dbo].[MilitaryRankTranslation] (
    [MilitaryRankId] INT            NOT NULL,
    [RankName]       NVARCHAR (50)  NOT NULL,
    [Description]    NVARCHAR (100) NULL,
    [LanguageId]     INT            NOT NULL,
    CONSTRAINT [FK_MilitaryRank_Language] FOREIGN KEY ([LanguageId]) REFERENCES [dbo].[Language] ([LanguageId]), 
    CONSTRAINT [PK_MilitaryRankTranslation] PRIMARY KEY ([MilitaryRankId], [LanguageId]), 
    CONSTRAINT [FK_MilitaryRankTranslation_MilitaryRank] FOREIGN KEY ([MilitaryRankId]) REFERENCES [dbo].[MilitaryRank]([MilitaryRankId])
);


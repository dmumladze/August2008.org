CREATE TABLE [dbo].[MilitaryGroupTranslation] (
    [MilitaryGroupId] INT			 NOT NULL,
    [GroupName]       NVARCHAR (100) NOT NULL,
    [Description]     NVARCHAR (250) NULL,
    [LanguageId]      INT            NOT NULL,
    CONSTRAINT [FK_MilitaryGroup_Languege] FOREIGN KEY ([LanguageId]) REFERENCES [dbo].[Language] ([LanguageId]), 
    CONSTRAINT [PK_MilitaryGroupTranslation] PRIMARY KEY ([MilitaryGroupId], [LanguageId]), 
    CONSTRAINT [FK_MilitaryGroupTranslation_MilitaryGroup] FOREIGN KEY ([MilitaryGroupId]) REFERENCES [dbo].[MilitaryGroup]([MilitaryGroupId])
);


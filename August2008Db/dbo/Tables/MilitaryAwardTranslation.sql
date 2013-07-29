CREATE TABLE [dbo].[MilitaryAwardTranslation] (
	[MilitaryAwardId] INT				NOT NULL, 
    [AwardName]		  NVARCHAR(100)		NOT NULL, 
    [Description]     NVARCHAR(1000)	NULL, 
    [LanguageId]	  INT				NOT NULL
	CONSTRAINT [FK_MilitaryAward_Language] FOREIGN KEY ([LanguageId]) REFERENCES [dbo].[Language] ([LanguageId]), 
    CONSTRAINT [PK_MilitaryAwardTranslation] PRIMARY KEY ([MilitaryAwardId], [LanguageId]), 
    CONSTRAINT [FK_MilitaryAwardTranslation_MilitaryAward] FOREIGN KEY ([MilitaryAwardId]) REFERENCES [dbo].[MilitaryAward]([MilitaryAwardId])
);

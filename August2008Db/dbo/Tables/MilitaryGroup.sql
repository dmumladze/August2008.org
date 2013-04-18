CREATE TABLE [dbo].[MilitaryGroup]
(
	[MilitaryGroupId] INT NOT NULL PRIMARY KEY, 
    [GroupName] NVARCHAR(50) NOT NULL, 
    [Description] NVARCHAR(250) NULL, 
    [LanguageId] INT NOT NULL, 
    CONSTRAINT [FK_MilitaryGroup_Languege] FOREIGN KEY ([LanguageId]) REFERENCES [dbo].[Language]([LanguageId])
)

CREATE TABLE [dbo].[Language]
(
	[LanguageId] INT NOT NULL PRIMARY KEY, 
    [DisplayName] NVARCHAR(50) NOT NULL, 
    [CultureName] NVARCHAR(50) NOT NULL
)

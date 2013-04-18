CREATE TABLE [dbo].[Language] (
    [LanguageId]  INT           IDENTITY (1, 1) NOT NULL,
    [DisplayName] NVARCHAR (50) NOT NULL,
    [EnglishName] NVARCHAR (50) NULL,
    [Culture]	  NVARCHAR(10)	NOT NULL, 
    CONSTRAINT [PK_Language] PRIMARY KEY CLUSTERED ([LanguageId] ASC)
);



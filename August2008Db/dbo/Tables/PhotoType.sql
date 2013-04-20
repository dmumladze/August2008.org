CREATE TABLE [dbo].[PhotoType] (
    [PhotoTypeId] INT            IDENTITY (1, 1) NOT NULL,
    [DisplayName] NVARCHAR (50)  NOT NULL,
    [Description] NVARCHAR (250) NULL, 
    CONSTRAINT [PK_PhotoType] PRIMARY KEY ([PhotoTypeId])
);


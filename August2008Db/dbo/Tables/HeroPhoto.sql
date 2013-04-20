CREATE TABLE [dbo].[HeroPhoto] (
    [HeroPhotoId] INT            IDENTITY (1, 1) NOT NULL,
    [PhotoUrl]    NVARCHAR (250) NOT NULL,
    [HeroId]      INT            NOT NULL,
    [PhotoTypeId] INT            NOT NULL,
    [DateCreated] DATETIME       NOT NULL,
    [UpdatedBy]   INT            NOT NULL,
    CONSTRAINT [PK_HeroPhoto] PRIMARY KEY CLUSTERED ([HeroPhotoId] ASC),
    CONSTRAINT [FK_HeroPhoto_Hero] FOREIGN KEY ([HeroId]) REFERENCES [dbo].[Hero] ([HeroId]),
    CONSTRAINT [FK_HeroPhoto_PhotoType] FOREIGN KEY ([PhotoTypeId]) REFERENCES [dbo].[PhotoType] ([PhotoTypeId]),
    CONSTRAINT [FK_HeroPhoto_User] FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[User] ([UserId])
);






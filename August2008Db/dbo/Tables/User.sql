﻿CREATE TABLE [dbo].[User] (
    [UserId]      INT           IDENTITY (1, 1) NOT NULL,
    [UserName]    NVARCHAR (50) NOT NULL,
    [Email]       NVARCHAR (50) NOT NULL,
    [DisplayName] NVARCHAR (50) NOT NULL,
    [DateCreated] DATETIME      NOT NULL, 
    CONSTRAINT [PK_User] PRIMARY KEY ([UserId])
);



CREATE TABLE [dbo].[Role]
(
	[RoleId]	  INT IDENTITY(1,1)   NOT NULL, 
    [Name]		  NVARCHAR(50)        NOT NULL, 
    [Description] NVARCHAR(50)        NOT NULL

    CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED ([RoleId] ASC));


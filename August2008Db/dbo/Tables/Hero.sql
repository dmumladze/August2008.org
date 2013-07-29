CREATE TABLE [dbo].[Hero] (
    [HeroId]          INT        IDENTITY (1, 1) NOT NULL,
    [Dob]			  DATETIME	 NULL,
	[Died]			  DATETIME	 NULL,
    [Version]         ROWVERSION NOT NULL,
	[UpdatedBy]		  INT		 NOT NULL	
    CONSTRAINT [PK_Hero] PRIMARY KEY CLUSTERED ([HeroId] ASC),
    CONSTRAINT [FK_User] FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[User]([UserId])
);



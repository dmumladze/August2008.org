CREATE TABLE [dbo].[Hero] (
    [HeroId]          INT        IDENTITY (1, 1) NOT NULL,
    [MilitaryRankId]  INT        NULL,
    [MilitaryGroupId] INT        NULL,
	[Dob]			  DATETIME	 NULL,
	[Died]			  DATETIME	 NULL,
    [Version]         ROWVERSION NOT NULL,
	[UpdatedBy]		  INT		 NOT NULL	
    CONSTRAINT [PK_Hero] PRIMARY KEY CLUSTERED ([HeroId] ASC),
    CONSTRAINT [FK_Hero_MilitaryGroup] FOREIGN KEY ([MilitaryGroupId]) REFERENCES [dbo].[MilitaryGroup] ([MilitaryGroupId]),
    CONSTRAINT [FK_Hero_MilitaryRank] FOREIGN KEY ([MilitaryRankId]) REFERENCES [dbo].[MilitaryRank] ([MilitaryRankId]),
	CONSTRAINT [FK_User] FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[User]([UserId])
);



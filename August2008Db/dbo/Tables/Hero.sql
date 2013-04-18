CREATE TABLE [dbo].[Hero]
(
	[HeroId]			INT NOT NULL PRIMARY KEY IDENTITY,
    [MilitaryRankId]	INT            NOT NULL,
    [MilitaryGroupId]	INT            NOT NULL,
    CONSTRAINT [FK_Hero_MilitaryGroup] FOREIGN KEY ([MilitaryGroupId]) REFERENCES [dbo].[MilitaryGroup] ([MilitaryGroupId]),
    CONSTRAINT [FK_Hero_MilitaryRank] FOREIGN KEY ([MilitaryRankId]) REFERENCES [dbo].[MilitaryRank] ([MilitaryRankId])
)

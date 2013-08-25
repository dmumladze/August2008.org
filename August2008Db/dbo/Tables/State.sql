CREATE TABLE [dbo].[State]
(
	[StateId]	INT	IDENTITY (1, 1) NOT NULL,
	[CountryId]	INT				NOT NULL,
    [Name]		NVARCHAR(10)	NULL, 
    [FullName]	NVARCHAR(50)	NULL,
	[Geo]		GEOGRAPHY		DEFAULT('POINT EMPTY')
	CONSTRAINT [PK_State] PRIMARY KEY CLUSTERED ([StateId] ASC)
	CONSTRAINT [FK_State_Country] FOREIGN KEY ([CountryId]) REFERENCES [dbo].[Country]([CountryId])
)

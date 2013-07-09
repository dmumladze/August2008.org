CREATE TABLE [dbo].[Donation]
(
	[DonationId]			INT IDENTITY(1, 1)	NOT NULL,
	[DonationProviderId]	INT					NOT NULL,
	[UserId]				INT					NOT NULL,
	[Amount]				MONEY				NOT NULL,
	[Currency]				NVARCHAR(10)		NULL,
	[Message]				NVARCHAR(500)		NULL,
	[DateDonated]			DATETIME			DEFAULT(GETDATE()),
	[AuthToken]				NVARCHAR(500)		NULL,
	CONSTRAINT [PK_Donation] PRIMARY KEY CLUSTERED ([DonationId] ASC),
	CONSTRAINT [FK_DonationProvider_User] FOREIGN KEY ([DonationProviderId]) REFERENCES [dbo].[DonationProvider] ([DonationProviderId]),
    CONSTRAINT [FK_Donation_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId])   
)

CREATE TABLE [dbo].[DonationSubscription]
(
	[DonationSubscriptionId]	INT IDENTITY(1, 1) NOT NULL, 
    [StartDate]					DATETIME NOT NULL, 
    [EndDate]					DATETIME NOT NULL, 
	[RecurrenceTimes]			INT NOT NULL,
    [SubscriptionId]			NVARCHAR(50) NOT NULL, 
    [Username]					NVARCHAR(50) NULL, 
    [Password]					NVARCHAR(50) NULL, 
    [UserId]					INT NOT NULL, 
    [ProviderData]				XML NULL
    CONSTRAINT [PK_DonationSubscription] PRIMARY KEY CLUSTERED ([DonationSubscriptionId] ASC),
    CONSTRAINT [FK_DonationSubscription_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId])
)

CREATE TABLE [dbo].[Donation] (
    [DonationId]         INT            IDENTITY (1, 1) NOT NULL,
    [DonationProviderId] INT            NOT NULL,
    [UserId]             INT            NOT NULL,
    [FirstName]          NVARCHAR (50)  NULL,
    [LastName]           NVARCHAR (50)  NULL,
    [Currency]           NVARCHAR (10)  NULL,
    [UserMessage]        NVARCHAR (500) NULL,
    [DateDonated]        DATETIME       DEFAULT (GETDATE()) NOT NULL,
    [Amount]             MONEY          NOT NULL,
    [ProviderData]       XML            NULL,
    [Email]              NVARCHAR (50)  NULL,
    CONSTRAINT [PK_Donation] PRIMARY KEY CLUSTERED ([DonationId] ASC),
    CONSTRAINT [FK_Donation_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId]),
    CONSTRAINT [FK_DonationProvider_User] FOREIGN KEY ([DonationProviderId]) REFERENCES [dbo].[DonationProvider] ([DonationProviderId])
);
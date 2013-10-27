CREATE TABLE [dbo].[DonationProvider] (
    [DonationProviderId] INT            IDENTITY (1, 1) NOT NULL,
    [ProviderName]       NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_DonationProvider] PRIMARY KEY CLUSTERED ([DonationProviderId] ASC)
);

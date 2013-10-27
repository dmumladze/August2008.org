CREATE PROCEDURE [dbo].[RefundDonation]
	@ExternalId		NVARCHAR(50),
	@ExternalStatus	NVARCHAR(25)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE dbo.Donation SET 
		IsCompleted		= 0,
		ExternalStatus	= @ExternalStatus
	OUTPUT
		deleted.DonationId,
		deleted.DonationProviderId,
		deleted.UserId,
		deleted.ExternalId,
		deleted.ExternalStatus,
		deleted.IsCompleted,
		deleted.Amount,
		deleted.Currency,
		deleted.UserMessage,
		deleted.ProviderData,
		deleted.CityId,
		deleted.StateId,
		deleted.CountryId,
		deleted.DonationSubscriptionId,
		deleted.TransactionType
	WHERE ExternalId = @ExternalId;
END;

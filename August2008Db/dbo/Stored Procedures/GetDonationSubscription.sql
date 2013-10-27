CREATE PROCEDURE [dbo].[GetDonationSubscription]
	@SubscriptionId	NVARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [DonationSubscriptionId]
		  ,[StartDate]
		  ,[EndDate]
		  ,[SubscriptionId]
		  ,[Username]
		  ,[Password]
		  ,[UserId]
	FROM [dbo].[DonationSubscription] WITH (NOLOCK)
	WHERE SubscriptionId = @SubscriptionId;
END;
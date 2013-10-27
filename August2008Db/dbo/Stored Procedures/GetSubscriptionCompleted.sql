CREATE PROCEDURE [dbo].[GetSubscriptionCompleted]
	@SubscriptionId	NVARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @result BIT = 0;

	IF EXISTS(SELECT TOP 1 1 FROM dbo.DonationSubscription WHERE SubscriptionId = @SubscriptionId)
	BEGIN
		SET @result = 1;	
	END;
	SELECT @result;
END;

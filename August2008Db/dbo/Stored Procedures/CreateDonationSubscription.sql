CREATE PROCEDURE [dbo].[CreateDonationSubscription]
    @StartDate				DATETIME,
    @EndDate				DATETIME,
	@RecurrenceTimes		INT,
    @SubscriptionId			NVARCHAR(50),
    @Username				NVARCHAR(50) = NULL,
    @Password				NVARCHAR(50) = NULL,
    @UserId					INT,
    @ProviderData			XML	= NULL,
	@DonationSubscriptionId	INT = NULL OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	IF EXISTS(SELECT TOP 1 1 FROM dbo.DonationSubscription WITH (NOLOCK) WHERE SubscriptionId = @SubscriptionId)
	BEGIN
		SELECT 
			@DonationSubscriptionId = DonationSubscriptionId
		FROM dbo.DonationSubscription WITH (NOLOCK)
		WHERE SubscriptionId = @SubscriptionId;
	END
	ELSE
	BEGIN
		INSERT INTO [dbo].[DonationSubscription]				
				   ([StartDate]
				   ,[EndDate]
				   ,[RecurrenceTimes]
				   ,[SubscriptionId]
				   ,[Username]
				   ,[Password]
				   ,[UserId]
				   ,[ProviderData])
			 VALUES
				   (@StartDate
				   ,@EndDate
				   ,@RecurrenceTimes
				   ,@SubscriptionId
				   ,@Username
				   ,@Password
				   ,@UserId
				   ,@ProviderData);
		SET @DonationSubscriptionId = SCOPE_IDENTITY();
	END
END;

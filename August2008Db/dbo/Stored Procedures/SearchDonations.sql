CREATE PROC [dbo].[SearchDonations]
	@StartDate		DATETIME,
	@EndDate		DATETIME
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		 dp.DonationProviderId
		,u.UserId 
		,d.DonationId
		,u.DisplayName
		,d.FirstName
		,d.LastName
		,d.Email
		,d.Amount
		,d.Currency
		,d.UserMessage
		,d.DateDonated				
	FROM dbo.Donation d (NOLOCK) 
	INNER JOIN dbo.[User] u (NOLOCK) ON d.UserId = u.UserId
	INNER JOIN dbo.DonationProvider dp (NOLOCK) ON d.DonationProviderId = dp.DonationProviderId
	WHERE d.DateDonated BETWEEN @StartDate AND @EndDate
END;
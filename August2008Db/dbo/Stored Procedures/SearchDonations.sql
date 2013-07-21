create PROC dbo.SearchDonations
	@StartDate		DATETIME,
	@EndDate		DATETIME
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		 d.DonationId
		,u.DisplayName
		,d.Amount
		,d.Currency
		,d.UserMessage
		,d.DateDonated				
	FROM dbo.Donation d (NOLOCK) 
	INNER JOIN dbo.[User] u (NOLOCK) ON d.UserId = u.UserId
	WHERE d.DateDonated BETWEEN @StartDate AND @EndDate
END;
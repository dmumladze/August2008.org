CREATE PROCEDURE [dbo].[GetDonationMessage]
	@DonationId	INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		UserMessage
	FROM dbo.Donation WITH (NOLOCK)
	WHERE DonationId = @DonationId;
END;
CREATE PROCEDURE [dbo].[UpdateUserMessage]
	@DonationId		INT,
	@UserMessage	NVARCHAR(500) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	 UPDATE dbo.Donation
	 SET UserMessage = @UserMessage
	 WHERE DonationId = @DonationId
END;


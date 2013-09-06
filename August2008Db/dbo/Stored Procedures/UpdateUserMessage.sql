CREATE PROCEDURE [dbo].[UpdateUserMessage]
	@DonationId		INT,
	@UserId			INT,
	@UserMessage	NVARCHAR(500) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	 UPDATE dbo.Donation
	 SET UserMessage = @UserMessage
	 WHERE DonationId = @DonationId
	 AND UserId = @UserId;
END;


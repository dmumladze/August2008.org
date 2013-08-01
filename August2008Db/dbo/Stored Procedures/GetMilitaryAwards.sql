CREATE PROCEDURE [dbo].[GetMilitaryAwards] 
	@LanguageId	INT = 1
AS
BEGIN
	SET NOCOUNT ON;
	SELECT MilitaryAwardId
		  ,AwardName
	FROM dbo.MilitaryAwardTranslation
	WHERE LanguageId = @LanguageId
	ORDER BY AwardName;
END;
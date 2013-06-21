CREATE PROCEDURE [dbo].[GetMilitaryRanks]
	@LanguageId	INT = 1
AS
BEGIN
	SET NOCOUNT ON;

	SELECT MilitaryRankId
		  ,RankName
		  ,Description
	FROM dbo.MilitaryRankTranslation
	WHERE LanguageId = @LanguageId
	ORDER BY RankName

END
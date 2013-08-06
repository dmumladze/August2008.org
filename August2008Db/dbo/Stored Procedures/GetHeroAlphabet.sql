CREATE PROCEDURE [dbo].[GetHeroAlphabet] 
	@LanguageId	INT = 1
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		DISTINCT(SUBSTRING(LastName, 1, 1)) AS FirstLetter
	FROM dbo.HeroTranslation WITH (NOLOCK)
	WHERE LanguageId = @LanguageId
	ORDER BY FirstLetter;
END;
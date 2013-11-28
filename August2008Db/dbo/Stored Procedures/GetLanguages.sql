CREATE PROCEDURE [dbo].[GetLanguages]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		LanguageId,
		DisplayName,
		EnglishName,
		Culture
	FROM dbo.[Language] WITH (NOLOCK)
END;

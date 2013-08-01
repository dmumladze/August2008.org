CREATE PROCEDURE [dbo].[GetMilitaryGroups]
	@LanguageId	INT = 1
AS
BEGIN
	SET NOCOUNT ON;
	SELECT MilitaryGroupId
		  ,GroupName
		  ,Description
	FROM dbo.MilitaryGroupTranslation
	WHERE LanguageId = @LanguageId
	ORDER BY GroupName;
END;

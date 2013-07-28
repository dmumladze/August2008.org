CREATE PROCEDURE dbo.GetRandomHero
	@LanguageId	INT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @RandomId	INT;
	DECLARE @HeroId		INT;

	DECLARE @Random TABLE
	(
		RandomId	INT IDENTITY,
		HeroId		INT
	);

	INSERT INTO @Random
	SELECT HeroId
	FROM dbo.HeroTranslation
	WHERE LanguageId = @LanguageId;

	SELECT 
		@RandomId = ROUND(RAND() * MAX(RandomId), 0)
	FROM @Random	
	
	SELECT 
		@HeroId	= HeroId
	FROM @Random	
	WHERE RandomId = @RandomId;
		
	EXEC dbo.GetHero @HeroId, @LanguageId; 
END;




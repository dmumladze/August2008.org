
CREATE PROCEDURE [dbo].[GetHero]
	@HeroId		INT,
	@LanguageId	INT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @Hero TABLE
	(
		HeroId			INT,
		MilitaryGroupId INT,
		MilitaryRankId	INT,
		Dob				DATETIME,
		Died			DATETIME,	
		FirstName		NVARCHAR(50),
		LastName		NVARCHAR(75),
		MiddleName		NVARCHAR(50),
		Biography		NVARCHAR(MAX),
		DateUpdated		DATETIME,
		UpdatedBy		INT,
		LanguageId		INT
	);

	INSERT INTO @Hero
	SELECT TOP 1
		h.HeroId,
		h.MilitaryGroupId,
		h.MilitaryRankId,
		h.Dob,	
		h.Died,		
		ht.FirstName,
		ht.LastName,
		ht.MiddleName,
		ht.Biography,
		ht.DateUpdated,
		ht.UpdatedBy,
		ht.LanguageId		
	FROM dbo.Hero h (NOLOCK) 
	INNER JOIN dbo.HeroTranslation ht (NOLOCK) ON h.HeroId = ht.HeroId AND ht.LanguageId = @LanguageId
	WHERE h.HeroId = @HeroId;

	SELECT * FROM @Hero;

	SELECT TOP 1
		mgt.MilitaryGroupId,
		mgt.GroupName,
		mgt.Description
	FROM dbo.MilitaryGroup mg (NOLOCK)
	INNER JOIN dbo.MilitaryGroupTranslation mgt (NOLOCK) ON mg.MilitaryGroupId = mgt.MilitaryGroupId AND mgt.LanguageId = @LanguageId
	INNER JOIN @Hero h ON mgt.MilitaryGroupId = h.MilitaryGroupId;

	SELECT TOP 1
		mrt.MilitaryRankId,
		mrt.RankName,
		mrt.Description
	FROM dbo.MilitaryRank mr (NOLOCK)
	INNER JOIN dbo.MilitaryRankTranslation mrt (NOLOCK) ON mr.MilitaryRankId = mrt.MilitaryRankId AND mrt.LanguageId = @LanguageId
	INNER JOIN @Hero h ON mrt.MilitaryRankId = h.MilitaryRankId;

	SELECT
		HeroPhotoId,
		PhotoUrl,
		HeroId,
		ContentType,
		IsThumbnail,
		DateCreated,
		UpdatedBy
	FROM dbo.HeroPhoto (NOLOCK)
	WHERE HeroId = @HeroId;
END;
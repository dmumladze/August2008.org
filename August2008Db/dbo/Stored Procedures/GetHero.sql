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
		MilitaryAwardId	INT,
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
		ht.MilitaryGroupId,
		ht.MilitaryRankId,
		ht.MilitaryAwardId,
		h.Dob,	
		h.Died,		
		ht.FirstName,
		ht.LastName,
		ht.MiddleName,
		ht.Biography,
		ht.DateUpdated,
		ht.UpdatedBy,
		ht.LanguageId		
	FROM dbo.Hero h WITH (NOLOCK) 
	INNER JOIN dbo.HeroTranslation ht WITH (NOLOCK) ON h.HeroId = ht.HeroId AND ht.LanguageId = @LanguageId
	WHERE h.HeroId = @HeroId;

	SELECT * FROM @Hero;

	SELECT TOP 1
		mgt.MilitaryGroupId,
		mgt.GroupName,
		mgt.Description
	FROM dbo.MilitaryGroup mg WITH (NOLOCK)
	INNER JOIN dbo.MilitaryGroupTranslation mgt WITH (NOLOCK) ON mg.MilitaryGroupId = mgt.MilitaryGroupId AND mgt.LanguageId = @LanguageId
	INNER JOIN @Hero h ON mgt.MilitaryGroupId = h.MilitaryGroupId;

	SELECT TOP 1
		mrt.MilitaryRankId,
		mrt.RankName,
		mrt.Description
	FROM dbo.MilitaryRank mr WITH (NOLOCK)
	INNER JOIN dbo.MilitaryRankTranslation mrt WITH (NOLOCK) ON mr.MilitaryRankId = mrt.MilitaryRankId AND mrt.LanguageId = @LanguageId
	INNER JOIN @Hero h ON mrt.MilitaryRankId = h.MilitaryRankId;

	SELECT TOP 1
		mat.MilitaryAwardId,
		mat.AwardName,
		mat.Description
	FROM dbo.MilitaryAward ma WITH (NOLOCK)
	INNER JOIN dbo.MilitaryAwardTranslation mat WITH (NOLOCK) ON ma.MilitaryAwardId = mat.MilitaryAwardId AND mat.LanguageId = @LanguageId
	INNER JOIN @Hero h ON mat.MilitaryAwardId = h.MilitaryAwardId;

	SELECT
		HeroPhotoId,
		PhotoUri,
		HeroId,
		ContentType,
		IsThumbnail,
		DateCreated,
		UpdatedBy
	FROM dbo.HeroPhoto WITH (NOLOCK)
	WHERE HeroId = @HeroId;
END;
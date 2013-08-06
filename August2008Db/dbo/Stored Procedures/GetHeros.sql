CREATE PROCEDURE dbo.GetHeros
	@PageNo		INT,
	@Name		NVARCHAR(50) = NULL,
	@PageSize	INT,
	@LanguageId	INT = 1,
	@TotalCount	INT = 0 OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @Hero TABLE
	(
		HeroId			INT,
		GroupName		NVARCHAR(100),
		RankName		NVARCHAR(100),
		AwardName		NVARCHAR(100),
		Dob				DATETIME,
		Died			DATETIME,	
		FirstName		NVARCHAR(50),
		LastName		NVARCHAR(75),
		MiddleName		NVARCHAR(50),
		Biography		NVARCHAR(MAX),
		DateUpdated		DATETIME
	);

	SELECT 
		@TotalCount = COUNT(*)
	FROM dbo.HeroTranslation ht WITH (NOLOCK)
	WHERE ht.LanguageId = @LanguageId
	AND (@Name IS NULL OR ht.LastName LIKE @Name + '%');

	INSERT INTO @Hero
	SELECT 
		h.HeroId,
		mgt.GroupName,
		mrt.RankName,
		mat.AwardName,
		h.Dob,
		h.Died,
		ht.FirstName,
		ht.LastName,
		ht.MiddleName,
		ht.Biography,
		ht.DateUpdated	
	FROM dbo.Hero h WITH (NOLOCK)
	INNER JOIN dbo.HeroTranslation ht WITH (NOLOCK) ON h.HeroId = ht.HeroId AND ht.LanguageId = @LanguageId
	LEFT JOIN dbo.MilitaryGroupTranslation mgt WITH (NOLOCK) ON ht.MilitaryGroupId = mgt.MilitaryGroupId AND mgt.LanguageId = @LanguageId
	LEFT JOIN dbo.MilitaryRankTranslation mrt WITH (NOLOCK) ON ht.MilitaryRankId = mrt.MilitaryRankId AND mrt.LanguageId = @LanguageId
	LEFT JOIN dbo.MilitaryAwardTranslation mat WITH (NOLOCK) ON ht.MilitaryAwardId = mat.MilitaryAwardId AND mat.LanguageId = @LanguageId
	AND (@Name IS NULL OR ht.LastName LIKE @Name + '%')
	ORDER BY ht.LastName, ht.FirstName
	OFFSET ((@PageNo - 1) * @PageSize) ROWS
	FETCH NEXT @PageSize ROWS ONLY

	SELECT
		HeroId,
		GroupName	AS [MilitaryGroup.GroupName],
		RankName	AS [MilitaryRank.RankName],
		AwardName	AS [MilitaryAward.AwardName],
		Dob,
		Died,
		FirstName,
		LastName,
		MiddleName,
		Biography,
		DateUpdated		
	FROM @Hero;

	SELECT 
		hp.HeroId,
		hp.ContentType,
		hp.PhotoUri,
		hp.IsThumbnail
	FROM dbo.HeroPhoto hp WITH (NOLOCK)
	INNER JOIN @Hero h ON hp.HeroId = h.HeroId
	--WHERE hp.IsThumbnail = 1;

END;
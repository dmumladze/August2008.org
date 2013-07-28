CREATE PROCEDURE [dbo].[GetHeros]
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
	FROM dbo.HeroTranslation ht (NOLOCK)
	WHERE ht.LanguageId = @LanguageId
	AND (@Name IS NULL OR ht.LastName LIKE @Name + '%');

	INSERT INTO @Hero
	SELECT 
		h.HeroId,
		mgt.GroupName,
		mrt.RankName,
		h.Dob,
		h.Died,
		ht.FirstName,
		ht.LastName,
		ht.MiddleName,
		ht.Biography,
		ht.DateUpdated	
	FROM dbo.Hero h (NOLOCK)
	INNER JOIN dbo.HeroTranslation ht (NOLOCK) ON h.HeroId = ht.HeroId AND ht.LanguageId = @LanguageId
	LEFT JOIN dbo.MilitaryGroupTranslation mgt (NOLOCK) ON h.MilitaryGroupId = mgt.MilitaryGroupId AND mgt.LanguageId = @LanguageId
	LEFT JOIN dbo.MilitaryRankTranslation mrt (NOLOCK) ON h.MilitaryRankId = mrt.MilitaryRankId AND mrt.LanguageId = @LanguageId
	AND (@Name IS NULL OR ht.LastName LIKE @Name + '%')
	ORDER BY ht.LastName, ht.FirstName
	OFFSET ((@PageNo - 1) * @PageSize) ROWS
	FETCH NEXT @PageSize ROWS ONLY

	SELECT
		HeroId,
		GroupName	AS [MilitaryGroup.GroupName],
		RankName	AS [MilitaryRank.RankName],
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
	FROM dbo.HeroPhoto hp (NOLOCK)
	INNER JOIN @Hero h ON hp.HeroId = h.HeroId
	--WHERE hp.IsThumbnail = 1;

END;
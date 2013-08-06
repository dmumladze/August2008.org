CREATE PROCEDURE [dbo].[UpdateHero]
	@HeroId				INT,
	@LanguageId			INT,
	@FirstName			NVARCHAR(50),
	@LastName			NVARCHAR(75),
	@MilitaryGroupId	INT				= NULL,
	@MilitaryRankId		INT				= NULL,
	@MilitaryAwardId	INT				= NULL,
	@Dob				DATETIME		= NULL,
	@Died				DATETIME		= NULL,	
	@MiddleName			NVARCHAR(50)	= NULL,
	@Biography			NVARCHAR(MAX)	= NULL,
	@Photos				XML				= NULL,
	@UpdatedBy			INT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE dbo.Hero 
	SET
		Dob			= ISNULL(@Dob, Dob),
		Died		= ISNULL(@Died, Died),
		UpdatedBy	= @UpdatedBy
	WHERE 
		HeroId		= @HeroId;

	UPDATE dbo.HeroTranslation 
	SET
		FirstName	= @FirstName,
		LastName	= @LastName,
		MiddleName	= ISNULL(@MiddleName, MiddleName),
		Biography	= ISNULL(@Biography, Biography),
		UpdatedBy	= @UpdatedBy,
		MilitaryGroupId	= ISNULL(@MilitaryGroupId, MilitaryGroupId),
		MilitaryRankId	= ISNULL(@MilitaryRankId, MilitaryRankId),
		MilitaryAwardId	= ISNULL(@MilitaryAwardId, MilitaryAwardId)
	WHERE 
		HeroId		= @HeroId
	AND LanguageId	= @LanguageId;

	IF (@Photos IS NOT NULL)
	BEGIN
		INSERT INTO dbo.HeroPhoto (
			HeroId,
			UpdatedBy,		
			PhotoUri,
			ContentType,
			IsThumbnail
		)
		SELECT  
			@HeroId,
			@UpdatedBy,	  
			T.c.value('./@PhotoUri', 'NVARCHAR(250)'),
			T.c.value('./@ContentType', 'NVARCHAR(25)'),
			T.c.value('./@IsThumbnail', 'BIT')
		FROM @Photos.nodes('/Photos/Photo') T(c)
	END;
END;
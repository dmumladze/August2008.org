CREATE PROCEDURE [dbo].[CreateHero]
	@MilitaryGroupId	INT				= NULL,	 
	@MilitaryRankId		INT				= NULL,
	@MilitaryAwardId	INT				= NULL,
	@Dob				DATETIME		= NULL,
	@Died				DATETIME		= NULL,	
	@FirstName			NVARCHAR(50),
	@LastName			NVARCHAR(75),
	@MiddleName			NVARCHAR(50)	= NULL,
	@Biography			NVARCHAR(MAX)	= NULL,
	@UpdatedBy			INT,
	@LanguageId			INT,
	@Photos				XML				= NULL,
	@HeroId				INT				= NULL	OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	IF	(EXISTS(SELECT 1 FROM dbo.HeroTranslation WITH (NOLOCK)
				WHERE FirstName = @FirstName
				AND	LastName = @LastName
				AND (@MilitaryGroupId IS NULL OR MilitaryGroupId = @MilitaryGroupId)
				AND (@MilitaryRankId IS NULL OR MilitaryRankId = @MilitaryRankId)
				AND (@MilitaryAwardId IS NULL OR MilitaryAwardId = @MilitaryAwardId)
				AND LanguageId = @LanguageId))
	BEGIN
		--RAISERROR(50001, 16, 1, @LastName, @FirstName); not supported in SQL Azure
		RAISERROR(N'Hero under the name of ''%s %s'' already exists.', 16, 1, @LastName, @FirstName);
		RETURN;
	END;	

	INSERT INTO dbo.Hero (
		Dob,
		Died,
		UpdatedBy
	)
	VALUES (
		@Dob,
		@Died,
		@UpdatedBy
	);

	SELECT @HeroId = SCOPE_IDENTITY();

	INSERT INTO dbo.HeroTranslation (
		HeroId,
		MilitaryRankId,
		MilitaryGroupId,
		MilitaryAwardId,
		LanguageId,
		FirstName,
		LastName,
		MiddleName,
		Biography,
		UpdatedBy
	)
	VALUES (
		@HeroId,
		@MilitaryRankId,
		@MilitaryGroupId,
		@MilitaryAwardId,
		@LanguageId,
		@FirstName,
		@LastName,
		@MiddleName,
		@Biography,
		@UpdatedBy
	);

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
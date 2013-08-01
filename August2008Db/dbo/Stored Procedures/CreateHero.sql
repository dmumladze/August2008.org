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

	IF	(EXISTS(SELECT 1 FROM dbo.HeroTranslation (NOLOCK)
				WHERE FirstName = @FirstName
				AND	LastName = @LastName
				AND (@MilitaryGroupId IS NULL OR MilitaryGroupId = @MilitaryGroupId)
				AND (@MilitaryRankId IS NULL OR MilitaryRankId = @MilitaryRankId)
				AND (@MilitaryAwardId IS NULL OR MilitaryAwardId = @MilitaryAwardId)
				AND LanguageId = @LanguageId))
	BEGIN
		RAISERROR(50001, 16, 1, @LastName, @FirstName);
		RETURN;
	END;	

	DECLARE @xdh INT;

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

	EXEC sp_xml_preparedocument @xdh OUTPUT, @Photos;

	INSERT INTO dbo.HeroPhoto (
		PhotoUri,
		HeroId,
		ContentType,
		IsThumbnail,
		UpdatedBy
	)
	SELECT    
		PhotoUri,
		@HeroId,
		ContentType,
		IsThumbnail,
		@UpdatedBy
	FROM OPENXML (@xdh, '/Photos/Photo', 1)
	WITH (
		PhotoUri	NVARCHAR(250),
		ContentType NVARCHAR(25),
		IsThumbnail BIT
	);
	EXEC sp_xml_removedocument @xdh;
END;
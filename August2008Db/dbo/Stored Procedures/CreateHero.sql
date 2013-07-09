
CREATE PROCEDURE [dbo].[CreateHero]
	@MilitaryGroupId	INT,
	@MilitaryRankId		INT,
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

	DECLARE @xdh INT;

	INSERT INTO dbo.Hero (
		MilitaryGroupId,
		MilitaryRankId,
		Dob,
		Died
	)
	VALUES (
		@MilitaryGroupId,
		@MilitaryRankId,
		@Dob,
		@Died
	);

	SELECT @HeroId = SCOPE_IDENTITY();

	INSERT INTO dbo.HeroTranslation (
		HeroId,
		LanguageId,
		FirstName,
		LastName,
		MiddleName,
		Biography,
		UpdatedBy
	)
	VALUES (
		@HeroId,
		@LanguageId,
		@FirstName,
		@LastName,
		@MiddleName,
		@Biography,
		@UpdatedBy
	);

	EXEC sp_xml_preparedocument @xdh OUTPUT, @Photos;

	INSERT INTO dbo.HeroPhoto (
		PhotoUrl,
		HeroId,
		ContentType,
		IsThumbnail,
		UpdatedBy
	)
	SELECT    
		PhotoUrl,
		@HeroId,
		ContentType,
		IsThumbnail,
		@UpdatedBy
	FROM OPENXML (@xdh, '/Photos/Photo', 1)
	WITH (
		PhotoUrl	NVARCHAR(250),
		ContentType NVARCHAR(25),
		IsThumbnail BIT
	);
	EXEC sp_xml_removedocument @xdh;
END;
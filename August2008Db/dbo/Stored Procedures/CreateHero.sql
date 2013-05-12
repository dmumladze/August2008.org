
create PROCEDURE [dbo].[CreateHero]
	@MilitaryGroupId	INT,
	@MilitaryRankId		INT,
	@Dob				DATETIME		= NULL,
	@Died				DATETIME		= NULL,	
	@FirstName			NVARCHAR(50),
	@LastName			NVARCHAR(75),
	@MiddleName			NVARCHAR(50)	= NULL,
	@Biography			NVARCHAR(MAX)	= NULL,
	@DateUpdated		DATETIME,
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
		DateUpdated,
		UpdatedBy
	)
	VALUES (
		@HeroId,
		@LanguageId,
		@FirstName,
		@LastName,
		@MiddleName,
		@Biography,
		@DateUpdated,
		@UpdatedBy
	);

	EXEC sp_xml_preparedocument @xdh OUTPUT, @Photos;

	INSERT INTO dbo.HeroPhoto (
		PhotoUrl,
		HeroId,
		PhotoTypeId,
		DateCreated,
		UpdatedBy
	)
	SELECT    
		PhotoUrl,
		@HeroId,
		PhotoTypeId,
		DateCreated,
		UpdatedBy
	FROM OPENXML (@xdh, '/Photos/Photo',2)
	WITH (
		PhotoUrl	NVARCHAR(250),
		PhotoTypeId INT,
		DateCreated DATETIME,
		UpdatedBy	INT
	);

	EXEC sp_xml_removedocument @xdh;
END;
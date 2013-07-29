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
END;
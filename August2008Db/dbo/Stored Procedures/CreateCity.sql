CREATE PROCEDURE [dbo].[CreateCity]
	@Name		NVARCHAR(50),
	@PostalCode	NVARCHAR(25),
	@Latitude	FLOAT,
	@Longitude	FLOAT,
	@StateId	INT				= NULL,
	@State		NVARCHAR(50)	= NULL,
	@CityId		INT				= NULL OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	IF ((@StateId IS NULL OR @StateId = 0) AND @State IS NOT NULL)
	BEGIN
		SELECT TOP 1
			@StateId = StateId 
		FROM dbo.[State] WITH (NOLOCK) 
		WHERE FullName = @State;
	END;

	INSERT INTO [dbo].[City]
		([StateId]
		,[Name]
		,[PostalCode]
		,[Geo])
	VALUES
		(@StateId
		,@Name
		,@PostalCode
		,geography::Point(@Latitude, @Longitude, 4326));

	SET @CityId = SCOPE_IDENTITY();
END;

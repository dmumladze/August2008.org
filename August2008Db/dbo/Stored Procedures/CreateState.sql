CREATE PROCEDURE [dbo].[CreateState]
	@Name		NVARCHAR(10),
	@FullName	NVARCHAR(50),
	@Latitude	FLOAT,
	@Longitude	FLOAT,
	@CountryId	INT				= NULL,
	@Country	NVARCHAR(50)	= NULL,
	@StateId	INT				= NULL OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	IF ((@CountryId IS NULL OR @CountryId = 0) AND @Country IS NOT NULL)
	BEGIN
		SELECT TOP 1
			@CountryId = CountryId 
		FROM dbo.Country WITH (NOLOCK) 
		WHERE FullName = @Country;
	END;

	INSERT INTO [dbo].[State]
		([CountryId]
		,[Name]
		,[FullName]
		,[Geo])
	VALUES
		(@CountryId
		,@Name
		,@FullName
		,geography::Point(@Latitude, @Longitude, 4326));

	SET @StateId = SCOPE_IDENTITY();
END;
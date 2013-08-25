CREATE PROCEDURE [dbo].[CreateCountry]
	@Name		NVARCHAR(10),
	@FullName	NVARCHAR(50),
	@Latitude	FLOAT,
	@Longitude	FLOAT,
	@CountryId	INT	= NULL OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[Country]
		([Name]
		,[FullName]
		,[Geo])
	VALUES
		(@Name
		,@FullName
		,geography::Point(@Latitude, @Longitude, 4326));

	SET @CountryId = SCOPE_IDENTITY();
END;

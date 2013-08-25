CREATE PROCEDURE [dbo].[UpdateUserProfileAddress]
	@UserId		INT,
	@Street		NVARCHAR(100),
	@CityId		INT,
	@StateId	INT,
	@Latitude	FLOAT,
	@Longitude	FLOAT,
	@CountryId	INT,
	@PostalCode	NVARCHAR(15) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT 1 FROM dbo.UserProfile WITH (NOLOCK) 
				  WHERE UserId = @UserId 
				  AND Street = @Street 
				  AND CityId = @CityId 
				  AND StateId = @StateId 
				  AND CountryId = @CountryId)
	BEGIN
		UPDATE dbo.UserProfile SET
			Street = @Street,
			CityId = @CityId,
			StateId = @StateId,
			CountryId = @CountryId,
			Geo = geography::Point(@Latitude, @Longitude, 4326)
		WHERE UserId = @UserId;
	END;

	IF NOT EXISTS(SELECT 1 FROM dbo.City WITH (NOLOCK) WHERE CityId = @CityId AND PostalCode = @PostalCode)
	BEGIN
		UPDATE dbo.City SET
			PostalCode = @PostalCode
		WHERE CityId = @CityId;
	END;
END;

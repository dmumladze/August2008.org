CREATE PROCEDURE [dbo].[UpdateUserProfileAddress]
	@UserId			INT,
	@Street			NVARCHAR(100) = NULL,
	@CityId			INT,
	@StateId		INT,
	@CountryId		INT,
	@Latitude		FLOAT	= NULL,
	@Longitude		FLOAT	= NULL
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
		UPDATE dbo.UserProfile 
		SET
			Street		= @Street,
			CityId		= @CityId,
			StateId		= @StateId,
			CountryId	= @CountryId,
			Geo			= dbo.GetGeoPoint(@Latitude, @Longitude)
		WHERE 
			UserId		= @UserId;
	END;
END;

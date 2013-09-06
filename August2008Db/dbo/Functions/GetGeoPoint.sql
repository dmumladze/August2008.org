CREATE FUNCTION dbo.GetGeoPoint
(
	@Latitude	FLOAT	= NULL,
	@Longitude	FLOAT	= NULL
)
RETURNS geography
AS
BEGIN
	DECLARE @point geography;

	IF @Latitude IS NULL OR @Longitude IS NULL BEGIN
		SET @point = 'POINT EMPTY';
		SET @point.STSrid = 4326;
	END
	ELSE BEGIN
		SET @point = geography::Point(@Latitude, @Longitude, 4326)
	END;

	RETURN @point;
END
CREATE PROCEDURE [dbo].[GetDonationsByState]
	@NwLat	FLOAT = NULL,
	@NwLng	FLOAT = NULL,
	@SeLat	FLOAT = NULL,
	@SeLng	FLOAT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @Location TABLE
	(
		CityId		INT,
		StateId		INT,
		CountryId	INT,
		Total		MONEY,
		ExtraData	INT
	);

	INSERT INTO @Location
	SELECT
		CityId,
		StateId,
		CountryId,
		SUM(Amount)	AS Total,
		COUNT(*)	AS ExtraData
	FROM dbo.Donation WITH(NOLOCK)
	WHERE CityId IS NOT NULL
	GROUP BY CityId, StateId, CountryId 

	SELECT
		ct.Name		AS City,
		cr.Name		AS Country,
		st.FullName	AS [State],
		loc.Total,
		loc.ExtraData,
		ct.Geo.Lat	AS Latitude,
		ct.Geo.Long	AS Longitude
	FROM dbo.City ct WITH(NOLOCK)
	INNER JOIN @Location loc ON ct.CityId = loc.CityId 
	INNER JOIN dbo.Country cr WITH(NOLOCK) ON loc.CountryId = cr.CountryId
	INNER JOIN dbo.[State] st WITH(NOLOCK) ON loc.StateId = st.StateId
	ORDER BY Total DESC;
END;
GO


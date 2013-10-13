CREATE PROCEDURE [dbo].[GetDonationsByCity]
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
		TotalSum	MONEY,
		TotalCount	INT
	);

	INSERT INTO @Location
	SELECT
		CityId,
		StateId,
		CountryId,
		SUM(Amount)	AS TotalSum,
		COUNT(*)	AS TotalCount
	FROM dbo.Donation WITH(NOLOCK)
	WHERE CityId IS NOT NULL
	GROUP BY CityId, StateId, CountryId 

	SELECT
		ct.Name		AS City,
		st.Name		AS [State],
		cr.FullName AS Country,
		loc.TotalSum,
		loc.TotalCount,
		ct.Geo.Lat	AS Latitude,
		ct.Geo.Long	AS Longitude
	FROM dbo.City ct WITH(NOLOCK)
	INNER JOIN @Location loc ON ct.CityId = loc.CityId 
	INNER JOIN dbo.Country cr WITH(NOLOCK) ON loc.CountryId = cr.CountryId
	INNER JOIN dbo.[State] st WITH(NOLOCK) ON loc.StateId = st.StateId
	ORDER BY TotalSum DESC;
END;
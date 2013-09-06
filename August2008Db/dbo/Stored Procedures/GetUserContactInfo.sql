CREATE PROCEDURE [dbo].[GetUserContactInfo]
	@UserId	INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		u.UserId,
		u.Email,
		u.DisplayName,
		up.Street		AS 'Address.Street',
		ct.Name			AS 'Address.City',
		st.Name			AS 'Address.State',
		ct.PostalCode	AS 'Address.PostalCode',
		cr.FullName		AS 'Address.Country',
		up.Geo.Lat		AS 'Address.Latitude',
		up.Geo.Long		AS 'Address.Longitude'
	FROM dbo.[User] u WITH (NOLOCK)
	INNER JOIN dbo.UserProfile up WITH (NOLOCK) ON u.UserId = up.UserId
	LEFT JOIN dbo.City ct WITH (NOLOCK) ON ct.CityId = up.CityId 
	LEFT JOIN dbo.[State] st WITH (NOLOCK) ON st.StateId = up.StateId
	LEFT JOIN dbo.Country cr WITH (NOLOCK) ON cr.CountryId = up.CountryId
	WHERE u.UserId = @UserId;
END;

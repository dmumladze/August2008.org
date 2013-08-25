CREATE PROCEDURE [dbo].[TryGetCity]
	@City		NVARCHAR(50),
	@State		NVARCHAR(50),
	@Country	NVARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TOP 1
		ct.CityId,
		st.StateId,
		ct.Name,
		ct.PostalCode
	FROM dbo.City ct WITH (NOLOCK)
	INNER JOIN dbo.[State] st WITH (NOLOCK) ON ct.StateId = st.StateId AND st.FullName = @State
	INNER JOIN dbo.Country cn WITH (NOLOCK) ON st.CountryId = cn.CountryId AND cn.FullName = @Country
	WHERE ct.Name = @City

	RETURN @@ROWCOUNT;
END;

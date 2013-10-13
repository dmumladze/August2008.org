CREATE PROCEDURE [dbo].[TryGetCity]
	@City		NVARCHAR(50),
	@State		NVARCHAR(50) = NULL,
	@PostalCode	NVARCHAR(15) = NULL,
	@Country	NVARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

	IF @State IS NOT NULL
	BEGIN
		SELECT TOP 1
			ct.CityId,
			ct.StateId,
			ct.Name,
			ct.PostalCode
		FROM dbo.City ct WITH(NOLOCK)
		INNER JOIN dbo.[State] st WITH (NOLOCK) ON ct.StateId = st.StateId AND st.Name = @State
		INNER JOIN dbo.Country cn WITH (NOLOCK) ON st.CountryId = cn.CountryId AND cn.FullName = @Country
		WHERE ct.Name = @City
	END
	ELSE IF @PostalCode IS NOT NULL 
	BEGIN
		SELECT TOP 1
			ct.CityId,
			ct.StateId,
			ct.Name,
			ct.PostalCode
		FROM dbo.City ct WITH(NOLOCK)
		WHERE ct.Name = @City
		AND ct.PostalCode = @PostalCode;
	END;

	RETURN @@ROWCOUNT;
END;
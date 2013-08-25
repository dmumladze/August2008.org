CREATE PROCEDURE [dbo].[TryGetCountry]
	@Country	NVARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TOP 1
		CountryId,
		Name,
		FullName
	FROM dbo.Country WITH (NOLOCK)
	WHERE FullName= @Country;

	RETURN @@ROWCOUNT;
END;

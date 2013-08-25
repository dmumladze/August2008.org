CREATE PROCEDURE [dbo].[TryGetState]
	@State		NVARCHAR(10),
	@Country	NVARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TOP 1
		s.StateId,
		c.CountryId,
		s.Name,
		s.FullName
	FROM dbo.[State] s WITH (NOLOCK)
	INNER JOIN dbo.Country c WITH (NOLOCK) ON s.CountryId = c.CountryId AND c.FullName = @Country
	WHERE s.Name = @State;

	RETURN @@ROWCOUNT;
END;

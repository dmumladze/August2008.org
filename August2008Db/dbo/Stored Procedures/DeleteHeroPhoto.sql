CREATE PROCEDURE [dbo].[DeleteHeroPhoto]
	@HeroPhotoId	INT
AS
BEGIN
	SET NOCOUNT ON;
	DELETE FROM dbo.HeroPhoto 
	OUTPUT 
		DELETED.HeroId,
		DELETED.HeroPhotoId,
		DELETED.PhotoUrl 
	WHERE HeroPhotoId = @HeroPhotoId;
END;

CREATE PROCEDURE [dbo].[GetArtistDetails]
    @artistID INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Get Artist Details
    SELECT 
        a.artistID, 
        a.title, 
        a.biography, 
        a.imageURL, 
        a.heroURL 
    FROM 
        dbo.Artist a
    WHERE 
        a.artistID = @artistID;

    -- Get Top Songs for the Artist
    SELECT TOP 5 
        s.songID,
        s.title as songTitle,
        alb.title as albumTitle,
        s.bpm,
        s.timeSignature,
        alb.imageURL as albumImageURL,
        s.multitracks,
        s.customMix,
        s.chart,
        s.rehearsalMix,
        s.patches,
        s.songSpecificPatches,
        s.proPresenter
    FROM 
        dbo.Song s
    INNER JOIN 
        dbo.Album alb ON s.albumID = alb.albumID
    WHERE 
        s.artistID = @artistID
    ORDER BY 
        s.dateCreation DESC;

    -- Get Albums for the Artist
    SELECT TOP 8 
        alb.albumID, 
        alb.title, 
        alb.imageURL, 
        alb.year
    FROM 
        dbo.Album alb 
    WHERE 
        alb.artistID = @artistID
    ORDER BY 
        alb.year DESC;
END
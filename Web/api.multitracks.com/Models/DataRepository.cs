using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DataAccess;

namespace api.multitracks.com.Models
{
    public class DataRepository
    {
        // Search for artists by name
        public List<Artist> SearchArtists(string searchTerm)
        {
            var artists = new List<Artist>();

            using (var sql = new SQL())
            {
                string query = @"
                    SELECT 
                        artistID, 
                        dateCreation,
                        title, 
                        biography, 
                        imageURL, 
                        heroURL 
                    FROM 
                        dbo.Artist 
                    WHERE 
                        title LIKE @searchTerm
                    ORDER BY 
                        title";

                sql.Parameters.Add("@searchTerm", SqlDbType.VarChar, "%" + searchTerm + "%");
                var dt = sql.ExecuteDT(query);

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        artists.Add(new Artist
                        {
                            ArtistID = Convert.ToInt32(row["artistID"]),
                            DateCreation = Convert.ToDateTime(row["dateCreation"]),
                            Title = row["title"].ToString(),
                            Biography = row["biography"].ToString(),
                            ImageURL = row["imageURL"].ToString(),
                            HeroURL = row["heroURL"].ToString()
                        });
                    }
                }
            }

            return artists;
        }

        // Get paginated list of songs
        public PagedResult<Song> GetSongs(int pageNumber, int pageSize)
        {
            var songs = new List<Song>();
            int totalCount = 0;

            using (var sql = new SQL())
            {
                // Get total count of songs
                string countQuery = "SELECT COUNT(*) FROM dbo.Song";
                totalCount = sql.ExecuteScalar<int>(countQuery);

                // Get paginated songs
                string query = @"
                    SELECT 
                        songID,
                        dateCreation,
                        albumID,
                        artistID,
                        title,
                        bpm,
                        timeSignature,
                        multitracks,
                        customMix,
                        chart,
                        rehearsalMix,
                        patches,
                        songSpecificPatches,
                        proPresenter
                    FROM 
                        dbo.Song
                    ORDER BY 
                        title
                    OFFSET @Offset ROWS
                    FETCH NEXT @PageSize ROWS ONLY";

                int offset = (pageNumber - 1) * pageSize;
                
                sql.Parameters.Add("@Offset", SqlDbType.Int, offset);
                sql.Parameters.Add("@PageSize", SqlDbType.Int, pageSize);
                
                var dt = sql.ExecuteDT(query);

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        songs.Add(new Song
                        {
                            SongID = Convert.ToInt32(row["songID"]),
                            DateCreation = Convert.ToDateTime(row["dateCreation"]),
                            AlbumID = Convert.ToInt32(row["albumID"]),
                            ArtistID = Convert.ToInt32(row["artistID"]),
                            Title = row["title"].ToString(),
                            Bpm = Convert.ToDecimal(row["bpm"]),
                            TimeSignature = row["timeSignature"].ToString(),
                            Multitracks = Convert.ToBoolean(row["multitracks"]),
                            CustomMix = Convert.ToBoolean(row["customMix"]),
                            Chart = Convert.ToBoolean(row["chart"]),
                            RehearsalMix = Convert.ToBoolean(row["rehearsalMix"]),
                            Patches = Convert.ToBoolean(row["patches"]),
                            SongSpecificPatches = Convert.ToBoolean(row["songSpecificPatches"]),
                            ProPresenter = Convert.ToBoolean(row["proPresenter"])
                        });
                    }
                }
            }

            return new PagedResult<Song>(songs, totalCount, pageNumber, pageSize);
        }

        // Add a new artist
        public int AddArtist(Artist artist)
        {
            int newArtistId = 0;

            using (var sql = new SQL())
            {
                string query = @"
                    INSERT INTO dbo.Artist (
                        title, 
                        biography, 
                        imageURL, 
                        heroURL
                    )
                    VALUES (
                        @Title, 
                        @Biography, 
                        @ImageURL, 
                        @HeroURL
                    );
                    SELECT SCOPE_IDENTITY();";

                sql.Parameters.Add("@Title", SqlDbType.VarChar, artist.Title);
                sql.Parameters.Add("@Biography", SqlDbType.VarChar, artist.Biography);
                sql.Parameters.Add("@ImageURL", SqlDbType.VarChar, artist.ImageURL);
                sql.Parameters.Add("@HeroURL", SqlDbType.VarChar, artist.HeroURL);

                object result = sql.ExecuteScalar<object>(query);
                if (result != null)
                {
                    newArtistId = Convert.ToInt32(result);
                }
            }

            return newArtistId;
        }
    }
}
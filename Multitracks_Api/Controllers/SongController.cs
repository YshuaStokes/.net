using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using DataAccess;
using Multitracks_Api.Models;

namespace Multitracks_Api.Controllers
{
    public class SongController : ApiController
    {
        [HttpGet]
        [Route("api/song/list")]
        public IHttpActionResult List(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                // Validate input parameters
                if (pageNumber < 1)
                {
                    return BadRequest("Page number must be greater than 0");
                }
                
                if (pageSize < 1 || pageSize > 100)
                {
                    return BadRequest("Page size must be between 1 and 100");
                }
                
                SQL sql = new SQL();
                
                // Calculate offset for pagination
                int offset = (pageNumber - 1) * pageSize;
                
                // Add parameters for pagination
                sql.Parameters.Add("@PageSize", SqlDbType.Int, pageSize);
                sql.Parameters.Add("@Offset", SqlDbType.Int, offset);
                
                // Query to get paginated songs with total count
                string query = @"
                    SELECT COUNT(*) OVER() AS TotalCount, 
                           s.songID, 
                           s.dateCreation, 
                           s.albumID, 
                           s.artistID, 
                           s.title, 
                           s.bpm, 
                           s.timeSignature, 
                           s.multitracks, 
                           s.customMix, 
                           s.chart, 
                           s.rehearsalMix, 
                           s.patches, 
                           s.songSpecificPatches, 
                           s.proPresenter,
                           a.title as artistTitle
                    FROM dbo.Song s
                    INNER JOIN dbo.Artist a ON s.artistID = a.artistID
                    ORDER BY s.title
                    OFFSET @Offset ROWS
                    FETCH NEXT @PageSize ROWS ONLY";
                
                DataTable dt = sql.ExecuteDT(query);
                
                List<Song> songs = new List<Song>();
                int totalCount = 0;
                
                // Process the query result
                for (int i = 0; i < DB.Rows(dt); i++)
                {
                    // Get total count from first row (it's the same for all rows)
                    if (i == 0)
                    {
                        totalCount = DB.Write<int>(dt, "TotalCount", i);
                    }
                    
                    Song song = new Song
                    {
                        SongID = DB.Write<int>(dt, "songID", i),
                        DateCreation = DB.Write<DateTime>(dt, "dateCreation", i),
                        AlbumID = DB.Write<int>(dt, "albumID", i),
                        ArtistID = DB.Write<int>(dt, "artistID", i),
                        Title = DB.Write<string>(dt, "title", i),
                        BPM = DB.Write<decimal>(dt, "bpm", i),
                        TimeSignature = DB.Write<string>(dt, "timeSignature", i),
                        Multitracks = DB.Write<bool>(dt, "multitracks", i),
                        CustomMix = DB.Write<bool>(dt, "customMix", i),
                        Chart = DB.Write<bool>(dt, "chart", i),
                        RehearsalMix = DB.Write<bool>(dt, "rehearsalMix", i),
                        Patches = DB.Write<bool>(dt, "patches", i),
                        SongSpecificPatches = DB.Write<bool>(dt, "songSpecificPatches", i),
                        ProPresenter = DB.Write<bool>(dt, "proPresenter", i)
                    };
                    
                    songs.Add(song);
                }
                
                // Calculate pagination information
                int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
                bool hasNext = pageNumber < totalPages;
                bool hasPrevious = pageNumber > 1;
                
                // Create paged response
                PagedResponseModel<Song> response = new PagedResponseModel<Song>
                {
                    Success = true,
                    Message = songs.Count > 0 ? "Songs retrieved successfully" : "No songs found",
                    Data = songs,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    TotalPages = totalPages,
                    HasNext = hasNext,
                    HasPrevious = hasPrevious
                };
                
                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
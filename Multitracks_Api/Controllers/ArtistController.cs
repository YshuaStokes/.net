using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Web.Http;
using DataAccess;
using Multitracks_Api.Models;

namespace Multitracks_Api.Controllers
{
    public class ArtistController : ApiController
    {
        [HttpGet]
        [Route("api/artist/search")]
        public IHttpActionResult Search(string name)
        {
            SQL sql = new SQL();
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    return BadRequest("Search name cannot be empty");
                }

                
                sql.Parameters.Add("@name", "%" + name + "%");
                
                DataTable dt = sql.ExecuteDT("SELECT artistID, dateCreation, title, biography, imageURL, heroURL " +
                                            "FROM dbo.Artist " +
                                            "WHERE title LIKE @name");
                
                List<Artist> artists = new List<Artist>();
                
                for (int i = 0; i < DB.Rows(dt); i++)
                {
                    Artist artist = new Artist
                    {
                        ArtistID = DB.Write<int>(dt, "artistID", i),
                        DateCreation = DB.Write<DateTime>(dt, "dateCreation", i),
                        Title = DB.Write<string>(dt, "title", i),
                        Biography = DB.Write<string>(dt, "biography", i),
                        ImageURL = DB.Write<string>(dt, "imageURL", i),
                        HeroURL = DB.Write<string>(dt, "heroURL", i)
                    };
                    
                    artists.Add(artist);
                }
                
                ResponseModel<List<Artist>> response = new ResponseModel<List<Artist>>
                {
                    Success = true,
                    Message = artists.Count > 0 ? "Artists found" : "No artists found with the given name",
                    Data = artists
                };
                
                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("api/artist/add")]
        public IHttpActionResult Add([FromBody] Artist artist)
        {
            try
            {
                // Validate input
                if (artist == null)
                {
                    return BadRequest("No artist data provided");
                }
                
                if (string.IsNullOrEmpty(artist.Title))
                {
                    return BadRequest("Artist title is required");
                }
                
                if (string.IsNullOrEmpty(artist.Biography))
                {
                    return BadRequest("Artist biography is required");
                }
                
                if (string.IsNullOrEmpty(artist.ImageURL))
                {
                    return BadRequest("Artist image URL is required");
                }
                
                if (string.IsNullOrEmpty(artist.HeroURL))
                {
                    return BadRequest("Artist hero URL is required");
                }
                
                SQL sql = new SQL();
                
                // Add parameters
                sql.Parameters.Add("@title", SqlDbType.VarChar, artist.Title);
                sql.Parameters.Add("@biography", SqlDbType.VarChar, artist.Biography);
                sql.Parameters.Add("@imageURL", SqlDbType.VarChar, artist.ImageURL);
                sql.Parameters.Add("@heroURL", SqlDbType.VarChar, artist.HeroURL);
                sql.Parameters.Add("@artistID", SqlDbType.Int);
                sql.Parameters["@artistID"].Direction = ParameterDirection.Output;

                // Execute the query to insert a new artist and get the ID back
                sql.Execute("INSERT INTO dbo.Artist (title, biography, imageURL, heroURL) " +
                                     "VALUES (@title, @biography, @imageURL, @heroURL); " +
                                     "SET @artistID = SCOPE_IDENTITY();");
                
                // Get the new artist ID
                int newArtistId = Convert.ToInt32(sql.Parameters["@artistID"].Value);
                artist.ArtistID = newArtistId;
                
                ResponseModel<Artist> response = new ResponseModel<Artist>
                {
                    Success = true,
                    Message = "Artist added successfully",
                    Data = artist
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
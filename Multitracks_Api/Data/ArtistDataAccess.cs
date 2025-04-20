using System;
using System.Collections.Generic;
using System.Data;
using DataAccess;
using Multitracks_Api.Models;

namespace Multitracks_Api.Data
{
    public class ArtistDataAccess : IArtistDataAccess
    {
        /// <summary>
        /// Searches for artists by name
        /// </summary>
        /// <param name="name">Artist name to search for</param>
        /// <returns>List of matching artists</returns>
        public ResponseModel<List<Artist>> SearchArtists(string name)
        {
            SQL sql = new SQL();
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
            
            return response;
        }

        /// <summary>
        /// Adds a new artist to the database
        /// </summary>
        /// <param name="artist">Artist to add</param>
        /// <returns>Added artist with ID</returns>
        public ResponseModel<Artist> AddArtist(Artist artist)
        {
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
            
            return response;
        }
    }
}
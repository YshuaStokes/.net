using System;
using System.Collections.Generic;
using System.Web.Http;
using Multitracks_Api.Data;
using Multitracks_Api.Models;

namespace Multitracks_Api.Controllers
{
    public class ArtistController : ApiController
    {
        private readonly IArtistDataAccess _artistDataAccess;

        public ArtistController()
        {
            // Initialize data access layer
            _artistDataAccess = new ArtistDataAccess();
        }

        [HttpGet]
        [Route("api/artist/search")]
        public IHttpActionResult Search(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    return BadRequest("Search name cannot be empty");
                }

                // Use the data access layer to search for artists
                var response = _artistDataAccess.SearchArtists(name);
                
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
                
                // Use the data access layer to add a new artist
                var response = _artistDataAccess.AddArtist(artist);
                
                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
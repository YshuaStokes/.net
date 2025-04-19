using System;
using System.Web.Http;
using api.multitracks.com.Models;

namespace api.multitracks.com.Controllers
{
    public class ArtistController : ApiController
    {
        private readonly DataRepository _repository;

        public ArtistController()
        {
            _repository = new DataRepository();
        }

        // GET: /artist/search?name=value
        [HttpGet]
        [Route("artist/search")]
        public IHttpActionResult Search(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return BadRequest("Search term is required");
                }

                var artists = _repository.SearchArtists(name);
                return Ok(ApiResponse<object>.SuccessResponse(new { artists }));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // POST: /artist/add
        [HttpPost]
        [Route("artist/add")]
        public IHttpActionResult Add(Artist artist)
        {
            try
            {
                if (artist == null)
                {
                    return BadRequest("Artist data is required");
                }

                if (string.IsNullOrWhiteSpace(artist.Title))
                {
                    return BadRequest("Artist title is required");
                }

                int newArtistId = _repository.AddArtist(artist);
                
                if (newArtistId > 0)
                {
                    artist.ArtistID = newArtistId;
                    return Ok(ApiResponse<Artist>.SuccessResponse(artist, "Artist created successfully"));
                }
                else
                {
                    return BadRequest("Failed to create artist");
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
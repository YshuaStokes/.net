using System;
using System.Web.Http;
using Multitracks_Api.Data;
using Multitracks_Api.Models;

namespace Multitracks_Api.Controllers
{
    public class SongController : ApiController
    {
        private readonly ISongDataAccess _songDataAccess;

        public SongController()
        {
            // Initialize data access layer
            _songDataAccess = new SongDataAccess();
        }

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
                
                // Use the data access layer to get songs
                var response = _songDataAccess.GetPagedSongs(pageNumber, pageSize);
                
                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
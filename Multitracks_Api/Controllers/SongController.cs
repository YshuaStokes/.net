using System;
using System.Web.Http;
using Multitracks_Api.Data;

namespace Multitracks_Api.Controllers
{
    public class SongController : ApiController
    {
        private readonly ISongDataAccess _songDataAccess;

        public SongController()
        {
            // Default constructor for normal operation
            _songDataAccess = new SongDataAccess();
        }

        public SongController(ISongDataAccess songDataAccess)
        {
            // Constructor for dependency injection (used in testing)
            _songDataAccess = songDataAccess ?? throw new ArgumentNullException(nameof(songDataAccess));
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
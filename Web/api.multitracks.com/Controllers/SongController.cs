using System;
using System.Web.Http;
using api.multitracks.com.Models;

namespace api.multitracks.com.Controllers
{
    public class SongController : ApiController
    {
        private readonly DataRepository _repository;

        public SongController()
        {
            _repository = new DataRepository();
        }

        // GET: /song/list?pageNumber=1&pageSize=10
        [HttpGet]
        [Route("song/list")]
        public IHttpActionResult List(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                // Validate and constrain page parameters
                if (pageNumber < 1) pageNumber = 1;
                if (pageSize < 1 || pageSize > 100) pageSize = 10;

                var pagedSongs = _repository.GetSongs(pageNumber, pageSize);
                
                return Ok(ApiResponse<PagedResult<Song>>.SuccessResponse(pagedSongs));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
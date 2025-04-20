using System.Collections.Generic;
using Multitracks_Api.Models;

namespace Multitracks_Api.Data
{
    /// <summary>
    /// Interface for song data access operations
    /// </summary>
    public interface ISongDataAccess : IDataAccess
    {
        /// <summary>
        /// Gets a paged list of songs
        /// </summary>
        /// <param name="pageNumber">Page number (1-based)</param>
        /// <param name="pageSize">Number of items per page</param>
        /// <returns>Paged list of songs with pagination metadata</returns>
        PagedResponseModel<Song> GetPagedSongs(int pageNumber, int pageSize);
    }
}
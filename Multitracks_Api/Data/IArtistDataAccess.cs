using System.Collections.Generic;
using Multitracks_Api.Models;

namespace Multitracks_Api.Data
{
    /// <summary>
    /// Interface for artist data access operations
    /// </summary>
    public interface IArtistDataAccess : IDataAccess
    {
        /// <summary>
        /// Searches for artists by name
        /// </summary>
        /// <param name="name">Artist name to search for</param>
        /// <returns>List of matching artists</returns>
        ResponseModel<List<Artist>> SearchArtists(string name);

        /// <summary>
        /// Adds a new artist to the database
        /// </summary>
        /// <param name="artist">Artist to add</param>
        /// <returns>Added artist with ID</returns>
        ResponseModel<Artist> AddArtist(Artist artist);
    }
}
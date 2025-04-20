using System;

namespace Multitracks_Api.Models
{
    public class Artist
    {
        public int ArtistID { get; set; }
        public DateTime DateCreation { get; set; }
        public string Title { get; set; }
        public string Biography { get; set; }
        public string ImageURL { get; set; }
        public string HeroURL { get; set; }
    }
}
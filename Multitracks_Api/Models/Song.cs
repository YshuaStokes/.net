using System;

namespace Multitracks_Api.Models
{
    public class Song
    {
        public int SongID { get; set; }
        public DateTime DateCreation { get; set; }
        public int AlbumID { get; set; }
        public int ArtistID { get; set; }
        public string Title { get; set; }
        public decimal BPM { get; set; }
        
        // Private field to store the raw time signature value
        private int _timeSignatureValue;
        
        // Original TimeSignature property (for backward compatibility)
        public string TimeSignature 
        { 
            get 
            {
                // Convert the numeric value to display format (4/4 or 6/8)
                return TimeSignatureExtensions.GetDisplayString(_timeSignatureValue);
            }
            set 
            {
                // Try to parse the string to an int if possible
                if (int.TryParse(value, out int result))
                {
                    _timeSignatureValue = result;
                }
            }
        }
        
        // New property to access and set the raw time signature value
        public int TimeSignatureValue
        {
            get { return _timeSignatureValue; }
            set { _timeSignatureValue = value; }
        }
        
        // Property to get the enum version of the time signature
        public TimeSignatureType TimeSignatureType
        {
            get { return TimeSignatureExtensions.FromInt(_timeSignatureValue); }
        }
        
        public bool Multitracks { get; set; }
        public bool CustomMix { get; set; }
        public bool Chart { get; set; }
        public bool RehearsalMix { get; set; }
        public bool Patches { get; set; }
        public bool SongSpecificPatches { get; set; }
        public bool ProPresenter { get; set; }
    }
}
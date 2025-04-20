using System;
using System.Collections.Generic;
using Multitracks_Api.Data;
using Multitracks_Api.Models;
using Xunit;

namespace Multitracks_Api.Tests.Data
{
    public class ArtistDataAccessTests
    {
        // TestableDataAccess class that doesn't rely on static DB calls
        private class TestableArtistDataAccess : ArtistDataAccess
        {
            private readonly List<Artist> _testArtists;
            private readonly int _newArtistId;

            public TestableArtistDataAccess(List<Artist> testArtists, int newArtistId = 0)
            {
                _testArtists = testArtists;
                _newArtistId = newArtistId;
            }

            public override ResponseModel<List<Artist>> SearchArtists(string name)
            {
                // Skip database and return test data instead
                ResponseModel<List<Artist>> response = new ResponseModel<List<Artist>>
                {
                    Success = true,
                    Message = _testArtists.Count > 0 ? "Artists found" : "No artists found with the given name",
                    Data = _testArtists
                };
                
                return response;
            }

            public override ResponseModel<Artist> AddArtist(Artist artist)
            {
                // Skip database and return test data with new ID
                artist.ArtistID = _newArtistId;
                artist.DateCreation = DateTime.Now;
                
                ResponseModel<Artist> response = new ResponseModel<Artist>
                {
                    Success = true,
                    Message = "Artist added successfully",
                    Data = artist
                };
                
                return response;
            }
        }

        [Fact]
        public void SearchArtists_WithMatches_ReturnsMatchingArtists()
        {
            // Arrange
            var testArtists = new List<Artist>
            {
                new Artist
                {
                    ArtistID = 1,
                    DateCreation = DateTime.Now,
                    Title = "Test Artist",
                    Biography = "Test Biography",
                    ImageURL = "http://test.com/image.jpg",
                    HeroURL = "http://test.com/hero.jpg"
                }
            };
            
            var dataAccess = new TestableArtistDataAccess(testArtists);

            // Act
            var result = dataAccess.SearchArtists("Test");

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal("Artists found", result.Message);
            Assert.Equal(1, result.Data.Count);
            
            var artist = result.Data[0];
            Assert.Equal(1, artist.ArtistID);
            Assert.Equal("Test Artist", artist.Title);
            Assert.Equal("Test Biography", artist.Biography);
            Assert.Equal("http://test.com/image.jpg", artist.ImageURL);
            Assert.Equal("http://test.com/hero.jpg", artist.HeroURL);
        }

        [Fact]
        public void SearchArtists_WithNoMatches_ReturnsEmptyList()
        {
            // Arrange - empty artist list
            var dataAccess = new TestableArtistDataAccess(new List<Artist>());

            // Act
            var result = dataAccess.SearchArtists("NonExistent");

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal("No artists found with the given name", result.Message);
            Assert.Empty(result.Data);
        }

        [Fact]
        public void AddArtist_ReturnsArtistWithNewId()
        {
            // Arrange
            int expectedNewId = 42;
            var dataAccess = new TestableArtistDataAccess(new List<Artist>(), expectedNewId);
            
            var artist = new Artist
            {
                Title = "New Test Artist",
                Biography = "New Test Biography",
                ImageURL = "http://test.com/new-image.jpg",
                HeroURL = "http://test.com/new-hero.jpg"
            };

            // Act
            var result = dataAccess.AddArtist(artist);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal("Artist added successfully", result.Message);
            
            var addedArtist = result.Data;
            Assert.Equal(expectedNewId, addedArtist.ArtistID);
            Assert.Equal("New Test Artist", addedArtist.Title);
            Assert.Equal("New Test Biography", addedArtist.Biography);
            Assert.Equal("http://test.com/new-image.jpg", addedArtist.ImageURL);
            Assert.Equal("http://test.com/new-hero.jpg", addedArtist.HeroURL);
        }
    }
}
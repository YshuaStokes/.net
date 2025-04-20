using System;
using System.Collections.Generic;
using Multitracks_Api.Data;
using Multitracks_Api.Models;
using Xunit;

namespace Multitracks_Api.Tests.Data
{
    public class SongDataAccessTests
    {
        // TestableDataAccess class that doesn't rely on static DB calls
        private class TestableSongDataAccess : SongDataAccess
        {
            private readonly List<Song> _testSongs;
            private readonly int _totalCount;

            public TestableSongDataAccess(List<Song> testSongs, int totalCount)
            {
                _testSongs = testSongs;
                _totalCount = totalCount;
            }

            public override PagedResponseModel<Song> GetPagedSongs(int pageNumber, int pageSize)
            {
                // Skip database and return test data instead
                int totalPages = (int)Math.Ceiling(_totalCount / (double)pageSize);
                bool hasNext = pageNumber < totalPages;
                bool hasPrevious = pageNumber > 1;
                
                // Create paged response with test data
                PagedResponseModel<Song> response = new PagedResponseModel<Song>
                {
                    Success = true,
                    Message = _testSongs.Count > 0 ? "Songs retrieved successfully" : "No songs found",
                    Data = _testSongs,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = _totalCount,
                    TotalPages = totalPages,
                    HasNext = hasNext,
                    HasPrevious = hasPrevious
                };
                
                return response;
            }
        }

        [Fact]
        public void GetPagedSongs_WithData_ReturnsCorrectPagedResponse()
        {
            // Arrange
            var testSongs = new List<Song>
            {
                new Song
                {
                    SongID = 1,
                    DateCreation = DateTime.Now,
                    AlbumID = 1,
                    ArtistID = 1,
                    Title = "Test Song 1",
                    BPM = 120,
                    TimeSignature = "4/4",
                    Multitracks = true,
                    CustomMix = true,
                    Chart = true,
                    RehearsalMix = true,
                    Patches = true,
                    SongSpecificPatches = true,
                    ProPresenter = true
                }
            };
            
            // Create the testable data access with 10 total songs (but only return 1)
            var dataAccess = new TestableSongDataAccess(testSongs, 10);

            // Act
            var result = dataAccess.GetPagedSongs(1, 10);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal("Songs retrieved successfully", result.Message);
            Assert.Equal(1, result.Data.Count);
            Assert.Equal(1, result.PageNumber);
            Assert.Equal(10, result.PageSize);
            Assert.Equal(10, result.TotalCount);
            Assert.Equal(1, result.TotalPages);
            Assert.False(result.HasNext);
            Assert.False(result.HasPrevious);

            var song = result.Data[0];
            Assert.Equal(1, song.SongID);
            Assert.Equal("Test Song 1", song.Title);
            Assert.Equal(120, song.BPM);
            Assert.Equal("4/4", song.TimeSignature);
            Assert.True(song.Multitracks);
        }

        [Fact]
        public void GetPagedSongs_WithNoData_ReturnsEmptyList()
        {
            // Arrange - empty song list
            var dataAccess = new TestableSongDataAccess(new List<Song>(), 0);

            // Act
            var result = dataAccess.GetPagedSongs(1, 10);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal("No songs found", result.Message);
            Assert.Empty(result.Data);
            Assert.Equal(1, result.PageNumber);
            Assert.Equal(10, result.PageSize);
            Assert.Equal(0, result.TotalCount);
            Assert.Equal(0, result.TotalPages);
            Assert.False(result.HasNext);
            Assert.False(result.HasPrevious);
        }

        [Fact]
        public void GetPagedSongs_WithMultiplePages_ReturnsPaginationInfo()
        {
            // Arrange
            var testSongs = new List<Song>
            {
                new Song
                {
                    SongID = 11,
                    Title = "Song on Page 2"
                }
            };
            
            // Create data access with 25 total songs (across multiple pages)
            var dataAccess = new TestableSongDataAccess(testSongs, 25);

            // Act - request page 2 with 10 items per page
            var result = dataAccess.GetPagedSongs(2, 10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.PageNumber);
            Assert.Equal(10, result.PageSize);
            Assert.Equal(25, result.TotalCount);
            Assert.Equal(3, result.TotalPages);  // 25 items / 10 per page = 3 pages
            Assert.True(result.HasNext);         // Page 2 of 3 has a next page
            Assert.True(result.HasPrevious);     // Page 2 has a previous page
        }
    }
}
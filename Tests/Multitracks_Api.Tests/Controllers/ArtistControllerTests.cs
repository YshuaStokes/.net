using System;
using System.Collections.Generic;
using System.Web.Http.Results;
using Moq;
using Multitracks_Api.Controllers;
using Multitracks_Api.Data;
using Multitracks_Api.Models;
using Xunit;

namespace Multitracks_Api.Tests.Controllers
{
    public class ArtistControllerTests
    {
        private readonly Mock<IArtistDataAccess> _mockArtistDataAccess;
        private readonly ArtistController _controller;

        public ArtistControllerTests()
        {
            // Setup mock for artist data access
            _mockArtistDataAccess = new Mock<IArtistDataAccess>();
            
            // Create a test version of the controller with the mock data access
            _controller = new ArtistController(_mockArtistDataAccess.Object);
        }

        [Fact]
        public void Search_WithValidName_ReturnsOkResult()
        {
            // Arrange
            string artistName = "Test Artist";
            var mockResponse = new ResponseModel<List<Artist>>
            {
                Success = true,
                Message = "Artists found",
                Data = new List<Artist>
                {
                    new Artist
                    {
                        ArtistID = 1,
                        Title = "Test Artist",
                        Biography = "Test Bio",
                        DateCreation = DateTime.Now,
                        ImageURL = "http://test.com/image.jpg",
                        HeroURL = "http://test.com/hero.jpg"
                    }
                }
            };
            
            _mockArtistDataAccess.Setup(x => x.SearchArtists(artistName)).Returns(mockResponse);

            // Act
            var result = _controller.Search(artistName);

            // Assert
            Assert.IsType<OkNegotiatedContentResult<ResponseModel<List<Artist>>>>(result);
            var okResult = result as OkNegotiatedContentResult<ResponseModel<List<Artist>>>;
            Assert.Equal(mockResponse, okResult.Content);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Search_WithEmptyName_ReturnsBadRequest(string artistName)
        {
            // Act
            var result = _controller.Search(artistName);

            // Assert
            Assert.IsType<BadRequestErrorMessageResult>(result);
            var badRequestResult = result as BadRequestErrorMessageResult;
            Assert.Equal("Search name cannot be empty", badRequestResult.Message);
        }

        [Fact]
        public void Search_WhenExceptionOccurs_ReturnsInternalServerError()
        {
            // Arrange
            _mockArtistDataAccess.Setup(x => x.SearchArtists(It.IsAny<string>()))
                .Throws(new Exception("Test exception"));

            // Act
            var result = _controller.Search("Test Artist");

            // Assert
            Assert.IsType<ExceptionResult>(result);
        }

        [Fact]
        public void Add_WithValidArtist_ReturnsOkResult()
        {
            // Arrange
            var artist = new Artist
            {
                Title = "Test Artist",
                Biography = "Test Biography",
                ImageURL = "http://test.com/image.jpg",
                HeroURL = "http://test.com/hero.jpg"
            };

            var mockResponse = new ResponseModel<Artist>
            {
                Success = true,
                Message = "Artist added successfully",
                Data = new Artist
                {
                    ArtistID = 1,
                    Title = "Test Artist",
                    Biography = "Test Biography",
                    DateCreation = DateTime.Now,
                    ImageURL = "http://test.com/image.jpg",
                    HeroURL = "http://test.com/hero.jpg"
                }
            };
            
            _mockArtistDataAccess.Setup(x => x.AddArtist(It.IsAny<Artist>())).Returns(mockResponse);

            // Act
            var result = _controller.Add(artist);

            // Assert
            Assert.IsType<OkNegotiatedContentResult<ResponseModel<Artist>>>(result);
            var okResult = result as OkNegotiatedContentResult<ResponseModel<Artist>>;
            Assert.Equal(mockResponse, okResult.Content);
        }

        [Fact]
        public void Add_WithNullArtist_ReturnsBadRequest()
        {
            // Act
            var result = _controller.Add(null);

            // Assert
            Assert.IsType<BadRequestErrorMessageResult>(result);
            var badRequestResult = result as BadRequestErrorMessageResult;
            Assert.Equal("No artist data provided", badRequestResult.Message);
        }

        [Theory]
        [InlineData(null, "Bio", "ImageURL", "HeroURL")]
        [InlineData("Title", null, "ImageURL", "HeroURL")]
        [InlineData("Title", "Bio", null, "HeroURL")]
        [InlineData("Title", "Bio", "ImageURL", null)]
        public void Add_WithMissingRequiredField_ReturnsBadRequest(string title, string biography, string imageURL, string heroURL)
        {
            // Arrange
            var artist = new Artist
            {
                Title = title,
                Biography = biography,
                ImageURL = imageURL,
                HeroURL = heroURL
            };

            // Act
            var result = _controller.Add(artist);

            // Assert
            Assert.IsType<BadRequestErrorMessageResult>(result);
        }

        [Fact]
        public void Add_WhenExceptionOccurs_ReturnsInternalServerError()
        {
            // Arrange
            var artist = new Artist
            {
                Title = "Test Artist",
                Biography = "Test Biography",
                ImageURL = "http://test.com/image.jpg",
                HeroURL = "http://test.com/hero.jpg"
            };

            _mockArtistDataAccess.Setup(x => x.AddArtist(It.IsAny<Artist>()))
                .Throws(new Exception("Test exception"));

            // Act
            var result = _controller.Add(artist);

            // Assert
            Assert.IsType<ExceptionResult>(result);
        }
    }
}
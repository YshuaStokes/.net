using System;
using System.Web.Http.Results;
using Moq;
using Multitracks_Api.Controllers;
using Multitracks_Api.Data;
using Multitracks_Api.Models;
using Xunit;

namespace Multitracks_Api.Tests.Controllers
{
    public class SongControllerTests
    {
        private readonly Mock<ISongDataAccess> _mockSongDataAccess;
        private readonly SongController _controller;

        public SongControllerTests()
        {
            // Setup mock for song data access
            _mockSongDataAccess = new Mock<ISongDataAccess>();
            
            // Create a test version of the controller with the mock data access
            _controller = new SongController(_mockSongDataAccess.Object);
        }

        [Fact]
        public void List_WithValidParameters_ReturnsOkResult()
        {
            // Arrange
            var mockResponse = new PagedResponseModel<Song>
            {
                Success = true,
                Message = "Songs retrieved successfully",
                Data = new System.Collections.Generic.List<Song>(),
                PageNumber = 1,
                PageSize = 10,
                TotalCount = 0,
                TotalPages = 0,
                HasNext = false,
                HasPrevious = false
            };

            _mockSongDataAccess.Setup(x => x.GetPagedSongs(1, 10)).Returns(mockResponse);

            // Act
            var result = _controller.List(1, 10);

            // Assert
            Assert.IsType<OkNegotiatedContentResult<PagedResponseModel<Song>>>(result);
            var okResult = result as OkNegotiatedContentResult<PagedResponseModel<Song>>;
            Assert.Equal(mockResponse, okResult.Content);
        }

        [Fact]
        public void List_WithInvalidPageNumber_ReturnsBadRequest()
        {
            // Arrange - invalid page number (0)
            int pageNumber = 0;
            int pageSize = 10;

            // Act
            var result = _controller.List(pageNumber, pageSize);

            // Assert
            Assert.IsType<BadRequestErrorMessageResult>(result);
            var badRequestResult = result as BadRequestErrorMessageResult;
            Assert.Equal("Page number must be greater than 0", badRequestResult.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(101)]
        public void List_WithInvalidPageSize_ReturnsBadRequest(int pageSize)
        {
            // Arrange
            int pageNumber = 1;

            // Act
            var result = _controller.List(pageNumber, pageSize);

            // Assert
            Assert.IsType<BadRequestErrorMessageResult>(result);
            var badRequestResult = result as BadRequestErrorMessageResult;
            Assert.Equal("Page size must be between 1 and 100", badRequestResult.Message);
        }

        [Fact]
        public void List_WhenExceptionOccurs_ReturnsInternalServerError()
        {
            // Arrange
            _mockSongDataAccess.Setup(x => x.GetPagedSongs(It.IsAny<int>(), It.IsAny<int>()))
                .Throws(new Exception("Test exception"));

            // Act
            var result = _controller.List(1, 10);

            // Assert
            Assert.IsType<ExceptionResult>(result);
        }
    }
}
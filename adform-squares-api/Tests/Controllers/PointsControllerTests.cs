using Microsoft.AspNetCore.Mvc;
using Moq;
using API.Domain.DTOs;
using API.Domain.Models;
using API.Domain.Interfaces.Services;
using API.Controllers;
using API.ViewModels;

namespace Tests.Controllers;

[TestClass]
public class PointsControllerTests
{
    private readonly Mock<IPointService> _pointServiceMock = new();

    private PointsController _controller;

    [TestInitialize]
    public void Initialize()
    {
        _controller = new PointsController(_pointServiceMock.Object);
    }

    [TestMethod]
    public async Task GetSquares_CallsDomainServiceAndReturnsSquares()
    {
        // Arrange
        var guid = Guid.NewGuid();

        var squares = new List<Square> { new([new PointDto(0, 0), new PointDto(1, 0), new PointDto(0, 1), new PointDto(1, 1)]) };

        _pointServiceMock.Setup(s => s.FindSquares(guid, CancellationToken.None)).ReturnsAsync(squares);

        // Act
        var response = await _controller.GetSquares(guid, CancellationToken.None);

        // Assert
        _pointServiceMock.Verify(s => s.FindSquares(guid, It.IsAny<CancellationToken>()), Times.Once());
        Assert.IsInstanceOfType<OkObjectResult>(response);
    }

    [TestMethod]
    public async Task AddPoint_CallsDomainServiceAndReturnsOk()
    {
        // Arrange
        var guid = Guid.NewGuid();

        var pointViewModel = new PointViewModel
        {
            X = 0,
            Y = 0
        };

        // Act
        var response = await _controller.AddPoint(guid, pointViewModel, CancellationToken.None);

        // Assert
        _pointServiceMock.Verify(s => 
            s.AddPoint(guid,
                It.Is<PointDto>(p => 
                    p.X == pointViewModel.X && 
                    p.Y == pointViewModel.Y),
                It.IsAny<CancellationToken>()), 
            Times.Once());

        Assert.IsInstanceOfType<OkObjectResult>(response);
    }

    [TestMethod]
    public async Task AddPoints_CallsDomainServiceAndReturnsOk()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var pointViewModel = new List<PointViewModel>();

        // Act
        var response = await _controller.AddPoints(guid, pointViewModel, CancellationToken.None);

        // Assert
        _pointServiceMock.Verify(s =>
            s.AddPoints(guid,
                It.IsAny<List<PointDto>>(),
                It.IsAny<CancellationToken>()),
            Times.Once());

        Assert.IsInstanceOfType<OkObjectResult>(response);
    }

    [TestMethod]
    public async Task DeletePoint_CallsDomainServiceAndReturnsOk()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var pointId = 5;

        // Act
        var response = await _controller.DeletePoint(pointId, CancellationToken.None);

        // Assert
        _pointServiceMock.Verify(s => s.DeletePoint(pointId, It.IsAny<CancellationToken>()), Times.Once());
        Assert.IsInstanceOfType<OkResult>(response);
    }
}

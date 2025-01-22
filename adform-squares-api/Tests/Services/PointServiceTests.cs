using API.Domain.Interfaces.Repositories;
using API.Domain.Exceptions;
using API.Domain.Constants;
using API.Domain.Services;
using API.Domain.Models;
using API.Domain.DTOs;
using Moq;

namespace Tests.Services;

[TestClass]
public class PointServiceTests
{
    private readonly Mock<IPointsRepository> _pointsRepository = new();
    private readonly Mock<IUserRepository> _userRepository = new();

    private PointService _service;

    [TestInitialize]
    public void Initialize()
    {
        _service = new PointService(_pointsRepository.Object, _userRepository.Object);
    }

    public static IEnumerable<object[]> PointTestData =>
    [
        [new[] { new Point(0, 0, 0), new Point(0, 1, 0), new Point(0, 0, 1), new Point(0, 1, 1), new Point(0, 0, 2), new Point(0, 1, 2) }, 2],
        [new[] { new Point(0, 0, 0), new Point(0, 1, 0), new Point(0, 0, 1), new Point(0, 1, 1) }, 1],
        [new[] { new Point(0, 24, 35), new Point(0, 19, 7), new Point(0, 4, 99), new Point(0, 2, 17) }, 0]
    ];

    [DataTestMethod]
    [DynamicData(nameof(PointTestData))]
    public async Task FindSquares_CallsRepoAndReturnsSquares(Point[] points, int squareCount)
    {
        // Assert
        var guid = Guid.NewGuid();

        _pointsRepository.Setup(p => p.GetAsync(guid, It.IsAny<CancellationToken>())).ReturnsAsync([.. points]);

        // Act
        var response = await _service.FindSquares(guid, CancellationToken.None);

        // Assert
        _pointsRepository.Verify(p => p.GetAsync(guid, It.IsAny<CancellationToken>()), Times.Once());
        Assert.AreEqual(squareCount, response.Count());
    }

    [TestMethod]
    public async Task AddPoint_UserExists_CallsRepo()
    {
        // Assert
        var guid = Guid.NewGuid();
        var user = new User(guid, "Joe");
        var point = new PointDto(0, 0);

        _userRepository.Setup(r => r.GetUserAsync(guid, It.IsAny<CancellationToken>())).ReturnsAsync(user);

        // Act
        await _service.AddPoint(guid, point, CancellationToken.None);

        // Assert
        _pointsRepository.Verify(p =>
            p.AddAsync(
                guid,
                It.Is<PointDto>(p => p.X == point.X && p.Y == point.Y),
            It.IsAny<CancellationToken>()),
            Times.Once());
    }

    [TestMethod]
    [ExpectedException(typeof(NotFoundException))]
    public async Task AddPoint_UserDoesntExist_ThrowsException()
    {
        // Assert
        var guid = Guid.NewGuid();
        var point = new PointDto(0, 0);

        // Act & Assert
        await _service.AddPoint(guid, point, It.IsAny<CancellationToken>());
    }

    [TestMethod]
    public async Task AddPoints_UserExistsAndValidCount_CallsRepo()
    {
        // Assert
        var guid = Guid.NewGuid();
        var user = new User(guid, "Joe");
        var point = new PointDto(0, 0);
        var points = new List<PointDto>() { point };

        _userRepository.Setup(r => r.GetUserAsync(guid, It.IsAny<CancellationToken>())).ReturnsAsync(user);

        // Act
        await _service.AddPoints(guid, points, CancellationToken.None);

        // Assert
        _pointsRepository.Verify(p =>
            p.AddRangeAsync(
                guid,
                It.Is<IEnumerable<PointDto>>(p => p.Count() == points.Count()),
            It.IsAny<CancellationToken>()),
            Times.Once());
    }

    [TestMethod]
    public async Task AddPoints_InvalidPointCount_ThrowsException()
    {
        // Assert
        var guid = Guid.NewGuid();
        var point = new PointDto(0, 0);
        var points = Enumerable.Repeat(point, PointConstants.MAX_POINTS_PER_IMPORT + 1).ToList();

        // Act & Assert
        await Assert.ThrowsExceptionAsync<ValidationException>(() => _service.AddPoints(guid, points, It.IsAny<CancellationToken>()));
    }

    [TestMethod]
    public async Task AddPoints_UserDoesntExist_ThrowsException()
    {
        // Assert
        var guid = Guid.NewGuid();
        var point = new PointDto(0, 0);
        var points = new List<PointDto>() { point };

        // Act & Assert
        await Assert.ThrowsExceptionAsync<NotFoundException>(() => _service.AddPoints(guid, points, It.IsAny<CancellationToken>()));
    }

    [TestMethod]
    public async Task DeletePoint_CallsRepo()
    {
        // Assert
        int pointId = 5;

        // Act
        await _service.DeletePoint(pointId, CancellationToken.None);

        // Assert
        _pointsRepository.Verify(p => p.DeleteAsync(pointId, It.IsAny<CancellationToken>()), Times.Once());
    }
}

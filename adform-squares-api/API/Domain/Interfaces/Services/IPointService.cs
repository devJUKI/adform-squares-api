using API.Domain.DTOs;
using API.Domain.Models;

namespace API.Domain.Interfaces.Services;

public interface IPointService
{
    Task<IEnumerable<Square>> FindSquares(Guid userId, CancellationToken cancellation);
    Task<Point> AddPoint(Guid userId, PointDto point, CancellationToken cancellation);
    Task<IEnumerable<Point>> AddPoints(Guid userId, List<PointDto> points, CancellationToken cancellation);
    Task DeletePoint(int pointId, CancellationToken cancellation);
}

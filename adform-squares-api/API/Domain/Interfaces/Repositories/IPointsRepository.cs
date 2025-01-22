using API.Domain.DTOs;
using API.Domain.Models;

namespace API.Domain.Interfaces.Repositories;

public interface IPointsRepository
{
    Task<List<Point>> GetAsync(Guid userId, CancellationToken cancellation);
    Task<Point> AddAsync(Guid userId, PointDto point, CancellationToken cancellation);
    Task<IEnumerable<Point>> AddRangeAsync(Guid userId, IEnumerable<PointDto> points, CancellationToken cancellation);
    Task DeleteAsync(int pointId, CancellationToken cancellation);
}

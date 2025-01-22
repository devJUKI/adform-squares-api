using API.Domain.Interfaces.Repositories;
using API.Domain.Interfaces.Services;
using API.Domain.Exceptions;
using API.Domain.Constants;
using API.Domain.Models;
using API.Domain.DTOs;

namespace API.Domain.Services;

public class PointService : IPointService
{
    private readonly IPointsRepository _pointsRepository;
    private readonly IUserRepository _userRepository;

    public PointService(IPointsRepository pointsRepository, IUserRepository userRepository)
    {
        _pointsRepository = pointsRepository;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<Square>> FindSquares(Guid userId, CancellationToken cancellation)
    {
        var points = await _pointsRepository.GetAsync(userId, cancellation);

        var pointsList = points
            .Select(p => new PointDto(p.X, p.Y))
            .ToList();

        var squares = new List<Square>();

        for (int i = 0; i < pointsList.Count - 3; i++)
        {
            for (int j = i + 1; j < pointsList.Count - 2; j++)
            {
                for (int k = j + 1; k < pointsList.Count - 1; k++)
                {
                    for (int l = k + 1; l < pointsList.Count; l++)
                    {
                        var p1 = pointsList[i];
                        var p2 = pointsList[j];
                        var p3 = pointsList[k];
                        var p4 = pointsList[l];

                        if (IsSquare(p1, p2, p3, p4))
                        {
                            squares.Add(new Square([p1, p2, p3, p4]));
                        }
                    }
                }
            }
        }

        return squares;
    }

    public async Task<Point> AddPoint(Guid userId, PointDto point, CancellationToken cancellation)
    {
        var user = await _userRepository.GetUserAsync(userId, cancellation)
            ?? throw new NotFoundException("User was not found");

        var pointResponse = await _pointsRepository.AddAsync(user.Id, point, cancellation);

        return pointResponse;
    }

    public async Task<IEnumerable<Point>> AddPoints(Guid userId, List<PointDto> points, CancellationToken cancellation)
    {
        if (points.Count > PointConstants.MAX_POINTS_PER_IMPORT)
            throw new ValidationException($"Max point count per import is {PointConstants.MAX_POINTS_PER_IMPORT}");

        var user = await _userRepository.GetUserAsync(userId, cancellation)
            ?? throw new NotFoundException("User was not found");

        var pointResponse = await _pointsRepository.AddRangeAsync(user.Id, points, cancellation);

        return pointResponse;
    }

    public async Task DeletePoint(int pointId, CancellationToken cancellation)
    {
        await _pointsRepository.DeleteAsync(pointId, cancellation);
    }

    private static bool IsSquare(PointDto p1, PointDto p2, PointDto p3, PointDto p4)
    {
        var dists = new List<int>
        {
            p1.DistanceSquared(p2),
            p1.DistanceSquared(p3),
            p1.DistanceSquared(p4),
            p2.DistanceSquared(p3),
            p2.DistanceSquared(p4),
            p3.DistanceSquared(p4)
        };

        dists.Sort();

        return dists[0] == dists[1] && dists[1] == dists[2] && dists[2] == dists[3] && dists[4] == dists[5];
    }
}

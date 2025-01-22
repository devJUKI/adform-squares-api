using API.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using API.Domain.Exceptions;
using API.Core.Entities;
using API.Domain.Models;
using API.Domain.DTOs;

namespace API.Infrastructure.Repositories;

public class PointsRepository : IPointsRepository
{
    private readonly SquaresDbContext _dbContext;

    public PointsRepository(SquaresDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Point>> GetAsync(Guid userId, CancellationToken cancellation)
    {
        return await _dbContext.Points
            .Where(p => p.UserId == userId)
            .Select(p => new Point(p.Id, p.X, p.Y))
            .ToListAsync(cancellation);
    }

    public async Task<Point> AddAsync(Guid userId, PointDto point, CancellationToken cancellation)
    {
        var alreadyExist = await _dbContext.Points
            .AnyAsync(p => p.UserId == userId && p.X == point.X && p.Y == point.Y, cancellation);

        if (alreadyExist)
            throw new ConflictException("Point with such coordinates already exists");

        var document = new PointDocument
        {
            X = point.X,
            Y = point.Y,
            UserId = userId
        };

        await _dbContext.Points.AddAsync(document, cancellation);
        await _dbContext.SaveChangesAsync(cancellation);

        return new Point(document.Id, document.X, document.Y);
    }

    public async Task<IEnumerable<Point>> AddRangeAsync(Guid userId, IEnumerable<PointDto> points, CancellationToken cancellation)
    {
        var existingPoints = await _dbContext.Points
            .Where(p => p.UserId == userId)
            .ToListAsync(cancellation);

        var uniquePoints = points
            .Where(p => !existingPoints.Any(ep => ep.X == p.X && ep.Y == p.Y))
            .Select(p => new PointDocument
            {
                X = p.X,
                Y = p.Y,
                UserId = userId
            }).ToList();

        await _dbContext.Points.AddRangeAsync(uniquePoints, cancellation);
        await _dbContext.SaveChangesAsync(cancellation);

        return uniquePoints.Select(p => new Point(p.Id, p.X, p.Y));
    }

    public async Task DeleteAsync(int pointId, CancellationToken cancellation)
    {
        var document = await _dbContext.Points.FindAsync(pointId, cancellation);

        if (document == null)
        {
            return;
        }

        _dbContext.Points.Remove(document);
        await _dbContext.SaveChangesAsync(cancellation);
    }
}

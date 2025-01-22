using API.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using API.Core.Entities;
using API.Domain.Models;

namespace API.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly SquaresDbContext _dbContext;

    public UserRepository(SquaresDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetUserAsync(Guid userId, CancellationToken cancellation)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == userId, cancellation);

        if (user == null)
            return null;

        return new User(user.Id, user.Name);
    }

    public async Task<User?> GetUserAsync(string name, CancellationToken cancellation)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Name == name, cancellation);

        if (user == null)
            return null;

        return new User(user.Id, user.Name);
    }

    public async Task<User> AddUserAsync(User user, CancellationToken cancellation)
    {
        var userDocument = new UserDocument
        {
            Id = user.Id,
            Name = user.Name
        };

        await _dbContext.Users.AddAsync(userDocument, cancellation);
        await _dbContext.SaveChangesAsync(cancellation);

        return new User(userDocument.Id, userDocument.Name);
    }
}

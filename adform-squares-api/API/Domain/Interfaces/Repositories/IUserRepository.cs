using API.Domain.Models;

namespace API.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserAsync(Guid userId, CancellationToken cancellation);
    Task<User?> GetUserAsync(string name, CancellationToken cancellation);
    Task<User> AddUserAsync(User user, CancellationToken cancellation);
}

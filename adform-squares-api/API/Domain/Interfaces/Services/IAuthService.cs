using API.Domain.Models;

namespace API.Domain.Interfaces.Services;

public interface IAuthService
{
    Task<User> Register(string name, CancellationToken cancellation);
}

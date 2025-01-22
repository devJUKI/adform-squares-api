using API.Domain.Interfaces.Repositories;
using API.Domain.Interfaces.Services;
using API.Domain.Exceptions;
using API.Domain.Models;

namespace API.Domain.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> Register(string name, CancellationToken cancellation)
    {
        if (string.IsNullOrEmpty(name))
            throw new ValidationException("Name can't be empty");

        var existingUser = await _userRepository.GetUserAsync(name, cancellation);

        if (existingUser != null)
            throw new ConflictException("User with this name already exists");

        var user = new User(name);
        var userResponse = await _userRepository.AddUserAsync(user, cancellation);

        return userResponse;
    }
}

using API.Domain.Interfaces.Repositories;
using API.Domain.Exceptions;
using API.Domain.Services;
using API.Domain.Models;
using Moq;

namespace Tests.Services;

[TestClass]
public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepository = new();

    private AuthService _service;

    [TestInitialize]
    public void Initialize()
    {
        _service = new AuthService(_userRepository.Object);
    }

    [TestMethod]
    public async Task Register_UserDoesntExist_CallsRepo()
    {
        // Assert
        string name = "Joe";

        // Act
        await _service.Register(name, CancellationToken.None);

        // Assert
        _userRepository.Verify(r => r.GetUserAsync(name, It.IsAny<CancellationToken>()), Times.Once);

        _userRepository.Verify(r =>
            r.AddUserAsync(
                It.Is<User>(u => u.Name == name),
                It.IsAny<CancellationToken>()),
            Times.Once());
    }

    [TestMethod]
    public async Task Register_EmptyName_ThrowsException()
    {
        // Assert
        var name = "";

        // Act & Assert
        await Assert.ThrowsExceptionAsync<ValidationException>(() => _service.Register(name, CancellationToken.None));
    }

    [TestMethod]
    public async Task Register_UserWithNameExist_ThrowsException()
    {
        // Assert
        var guid = Guid.NewGuid();
        var user = new User(guid, "Joe");

        _userRepository.Setup(r => r.GetUserAsync(user.Name, It.IsAny<CancellationToken>())).ReturnsAsync(user);

        // Act & Assert
        await Assert.ThrowsExceptionAsync<ConflictException>(() => _service.Register(user.Name, CancellationToken.None));
    }
}

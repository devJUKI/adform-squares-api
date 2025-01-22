using API.Controllers;
using API.Domain.Interfaces.Services;
using API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests.Controllers;

[TestClass]
public class UsersControllerTests
{
    private readonly Mock<IAuthService> _authServiceMock = new();

    private UsersController _controller;

    [TestInitialize]
    public void Initialize()
    {
        _controller = new UsersController(_authServiceMock.Object);
    }

    [TestMethod]
    public async Task Register_CallsDomainServiceAndReturnsOk()
    {
        // Arrange
        var viewModel = new RegisterViewModel
        {
            Name = "Joe"
        };

        // Act
        var response = await _controller.Register(viewModel, CancellationToken.None);

        // Assert
        _authServiceMock.Verify(s => s.Register(viewModel.Name, It.IsAny<CancellationToken>()), Times.Once());
        Assert.IsInstanceOfType<OkObjectResult>(response);
    }
}

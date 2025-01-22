using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using API.Domain.Interfaces.Services;
using API.ViewModels;

namespace API.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{apiVersion:apiVersion}")]
public class UsersController : ControllerBase
{
    private readonly IAuthService _authService;

    public UsersController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterViewModel viewModel, CancellationToken cancellation)
    {
        var user = await _authService.Register(viewModel.Name, cancellation);

        return Ok(user);
    }
}

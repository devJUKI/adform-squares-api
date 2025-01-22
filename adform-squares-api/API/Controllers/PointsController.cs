using Microsoft.AspNetCore.Mvc;
using API.Domain.DTOs;
using Asp.Versioning;
using API.Domain.Interfaces.Services;
using API.ViewModels;

namespace API.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{apiVersion:apiVersion}/users/{userId:Guid}")]
public class PointsController : ControllerBase
{
    private readonly IPointService _pointService;

    public PointsController(IPointService pointService)
    {
        _pointService = pointService;
    }

    [HttpGet("squares")]
    public async Task<IActionResult> GetSquares(Guid userId, CancellationToken cancellation)
    {
        var response = await _pointService.FindSquares(userId, cancellation);

        return Ok(response);
    }

    [HttpPost("points")]
    public async Task<IActionResult> AddPoint(Guid userId, [FromBody] PointViewModel viewModel, CancellationToken cancellation)
    {
        var point = new PointDto(viewModel.X, viewModel.Y);
        var response = await _pointService.AddPoint(userId, point, cancellation);

        return Ok(response);
    }

    [HttpPost("points/import")]
    public async Task<IActionResult> AddPoints(Guid userId, [FromBody] List<PointViewModel> viewModel, CancellationToken cancellation)
    {
        var points = viewModel.Select(v => new PointDto(v.X, v.Y)).ToList();
        var response = await _pointService.AddPoints(userId, points, cancellation);

        return Ok(response);
    }

    [HttpDelete("points/{pointId}")]
    public async Task<IActionResult> DeletePoint(int pointId, CancellationToken cancellation)
    {
        await _pointService.DeletePoint(pointId, cancellation);

        return Ok();
    }
}

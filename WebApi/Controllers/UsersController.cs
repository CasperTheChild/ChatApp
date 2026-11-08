using Application.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class UsersController
    : ControllerBase
{
    private readonly UserService userService;

    public UsersController(UserService userService)
    {
        this.userService = userService;
    }

    [HttpGet("username/{username}")]
    public async Task<IActionResult> SearchUsersAsync(
        string username,
        [FromQuery] int? limit,
        [FromQuery] string? cursor)
    {
        return Ok(await userService.SearchUsersAsync(username, limit, cursor));
    }
}

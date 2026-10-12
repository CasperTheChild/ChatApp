using Microsoft.AspNetCore.Mvc;
using Application.IAM.Auth;
using Application.IAM;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly AuthenticationService authenticationService;
    private readonly CurrentUserService currentUserService;

    public AuthenticationController(AuthenticationService authenticationService, CurrentUserService currentUserService)
    {
        this.authenticationService = authenticationService;
        this.currentUserService = currentUserService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        var token = await authenticationService.LoginAsync(model.UserName, model.Password);
        return Ok(token);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
        var result = await authenticationService.RegisterAsync(model.UserName, model.Password);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetUserId()
    {
        var result = currentUserService.GetCurrentUserId();
        return Ok(result);
    }
}

using Microsoft.AspNetCore.Mvc;
using Application.DTOs.Auth;
using Application.Services;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly AuthenticationService authenticationService;

    public AuthenticationController(AuthenticationService authenticationService)
    {
        this.authenticationService = authenticationService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var token = await authenticationService.LoginAsync(model.UserName, model.Password);
        return Ok(token);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        var result = await authenticationService.RegisterAsync(model.UserName, model.Password);
        return Ok(result);
    }
}

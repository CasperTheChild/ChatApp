using Application.Services.Interface;

namespace Application.Services;

public class AuthenticationService
{
    private readonly IAuthService authService;

    public AuthenticationService(IAuthService authService)
    {
        this.authService = authService;
    }

    public async Task<string?> LoginAsync(string username, string password)
    {
        return await authService.LoginAsync(username, password);
    }

    public async Task<bool> RegisterAsync(string username, string password)
    {
        return await authService.RegisterAsync(username, password);
    }
}

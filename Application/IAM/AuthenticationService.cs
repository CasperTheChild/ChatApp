using Application.IAM.Results;
using Application.IAM.Services.Interface;

namespace Application.IAM;

public class AuthenticationService
{
    private readonly IAuthenticationService authService;

    public AuthenticationService(IAuthenticationService authService)
    {
        this.authService = authService;
    }

    public async Task<AuthenticationResult?> LoginAsync(string username, string password)
    {
        return await authService.LoginAsync(username, password);
    }

    public async Task<bool> RegisterAsync(string username, string password)
    {
        return await authService.RegisterAsync(username, password);
    }
}

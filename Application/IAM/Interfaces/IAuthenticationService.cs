using Application.IAM.Results;

namespace Application.IAM.Services.Interface;

public interface IAuthenticationService
{
    Task<AuthenticationResult?> LoginAsync(string username, string password);
    Task<bool> RegisterAsync(string username, string password);
}

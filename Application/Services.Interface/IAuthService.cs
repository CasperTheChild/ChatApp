namespace Application.Services.Interface;

public interface IAuthService
{
    Task<string?> LoginAsync(string username, string password);
    Task<bool> RegisterAsync(string username, string password);
}

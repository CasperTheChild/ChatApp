using Application.Services.Interface;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Drawing.Text;
using System.IdentityModel.Tokens.Jwt;

namespace Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly IConfiguration configuration;

    public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        this.userManager = userManager;
        this.configuration = configuration;
    }

    public async Task<string?> LoginAsync(string username, string password)
    {
        var user = await userManager.FindByNameAsync(username);

        if (user == null)
        {
            return null;
        }

        if (!await userManager.CheckPasswordAsync(user, password))
        {
            return null;
        }

        return GenerateJwtToken(user);
    }

    public async Task<bool> RegisterAsync(string username, string password)
    {
        var user = new ApplicationUser { UserName = username };

        var result = await userManager.CreateAsync(user, password);


        Debug.WriteLine($"Succeeded: {result.Succeeded}");
        Debug.WriteLine($"Error count: {result.Errors.Count()}");

        foreach (var error in result.Errors)
        {
            Debug.WriteLine($"{error.Code}: {error.Description}");
        }

        return result.Succeeded;
    }

    private string GenerateJwtToken(ApplicationUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = System.Text.Encoding.ASCII.GetBytes(configuration["Jwt:Key"]!);
        var tokenDescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
        {
            Subject = new System.Security.Claims.ClaimsIdentity(new[]
            {
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, user.UserName!)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"],
            SigningCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(
                new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
                Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}

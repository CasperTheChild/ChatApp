using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Auth;

public class LoginModel
{
    [Required]
    public string UserName { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
}

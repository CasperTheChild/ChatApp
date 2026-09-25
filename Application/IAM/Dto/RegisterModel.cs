using System.ComponentModel.DataAnnotations;

namespace Application.IAM.Auth;

public class RegisterModel
{
    [Required]
    public string UserName { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
    [Compare("Password")]
    public string ConfirmPassword { get; set; } = string.Empty;
}

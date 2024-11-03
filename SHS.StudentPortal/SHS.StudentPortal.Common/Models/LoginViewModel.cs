using System.ComponentModel.DataAnnotations;

namespace SHS.StudentPortal.Common.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Please select a role before logging in")]
    public string UserAccountType { get; set; }

    [Required(ErrorMessage = "Username cannot be empty")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Password cannot be empty")]
    public string Password { get; set; }
}

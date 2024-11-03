namespace SHS.StudentPortal.Common.Models;

public class UserPrincipalViewModel
{
    public Guid UserId { get; set; }

    public string Username { get; set; }

    public string Firstname { get; set; }

    public string? Middlename { get; set; }

    public string Lastname { get; set; }

    public string RoleType { get; set; }
}

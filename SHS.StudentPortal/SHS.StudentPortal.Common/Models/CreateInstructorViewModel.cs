namespace SHS.StudentPortal.Common.Models;

public class CreateInstructorViewModel
{
    public string EmployeeId { get; set; }

    public string Major { get; set; }

    public int? DepartmentId { get; set; }

    public Guid? AdvisorySectionId { get; set; }

    public string FirstName { get; set; }

    public string? MiddleName { get; set; }

    public string LastName { get; set; }

    public string ContactInfo { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public string EmailAddress { get; set; }

    public static CreateInstructorViewModel New()
    {
        return new CreateInstructorViewModel
        {
            EmployeeId = string.Empty,
            Major = string.Empty,
            DepartmentId = null,
            AdvisorySectionId = null,
            FirstName = string.Empty,
            MiddleName = string.Empty,
            LastName = string.Empty,
            ContactInfo = string.Empty,
            Username = string.Empty,
            Password = string.Empty,
            EmailAddress = string.Empty
        };
    }
}

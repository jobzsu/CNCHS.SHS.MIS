namespace SHS.StudentPortal.Common.Models;

public class CreateInstructorViewModel
{
    public string EmployeeId { get; set; }

    public string Major { get; set; }

    public int DepartmentId { get; set; }

    public Guid AdvisorySectionId { get; set; }

    public string FirstName { get; set; }

    public string? MiddleName { get; set; }

    public string LastName { get; set; }

    public string ContactInfo { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public string EmailAddress { get; set; }
}

namespace SHS.StudentPortal.Common.Models;

public class UpdateInstructorViewModel
{
    public string Major { get; set; }

    public int DepartmentId { get; set; }

    public Guid AdvisorySectionId { get; set; }

    public string FirstName { get; set; }

    public string MiddleName { get; set; }

    public string LastName { get; set; }

    public string ContactInfo { get; set; }
}

namespace SHS.StudentPortal.Common.Models;

public class BaseInstructorViewModel
{
    public Guid Id { get; set; }

    public string EmployeeId { get; set; }

    public string Major { get; set; }

    public string FirstName { get; set; }

    public string? MiddleName { get; set; }

    public string LastName { get; set; }

    public string ContactInfo { get; set; }

    public List<KeyValuePair<int, string>> DepartmentList { get; set; }

    public int DepartmentId { get; set; }

    public Guid AdvisorySectionId { get; set; }

    public List<KeyValuePair<Guid, string>> SectionList { get; set; }
}

public class Schedules
{
    public string Subject { get; set; }

    public string Days { get; set; }

    public string Time { get; set; }

    public string RoomNumber { get; set; }

    public string GradesSubmitted { get; set; }
}

public class InstructorViewModel : BaseInstructorViewModel
{
    public string Username { get; set; }

    public string LastLogin { get; set; }

    public string CurrentSemester { get; set; }

    public string CurrentAcademicYear { get; set; }

    public List<Schedules> Schedules { get; set; }
}

public class ForCreateInstructorViewModel : BaseInstructorViewModel 
{
    // Login Info
    public string Username { get; set; }

    public string Password { get; set; }

    public string ConfirmPassword { get; set; }

    public string EmailAddress { get; set; }
};


public class InstructorInfoAdminViewDTO
{
    public Guid Id { get; set; }

    public string EmployeeId { get; set; }

    public string Major { get; set; }

    public string FirstName { get; set; }

    public string? MiddleName { get; set; }

    public string LastName { get; set; }

    public string ContactInfo { get; set; }

    public int? DepartmentId { get; set; }

    public Guid? AdvisorySectionId { get; set; }

    public string Username { get; set; }

    public DateTime? LastLogin { get; set; }

    public List<Schedules> Schedules { get; set; }

    public static InstructorInfoAdminViewDTO New()
    {
        return new InstructorInfoAdminViewDTO()
        {
            Id = Guid.Empty,
            EmployeeId = "NEW",
            Major = string.Empty,
            FirstName = string.Empty,
            MiddleName = string.Empty,
            LastName = string.Empty,
            ContactInfo = string.Empty,
            DepartmentId = null,
            AdvisorySectionId = null,
            Username = string.Empty,
            LastLogin = null,
            Schedules = new List<Schedules>()
        };
    }
}
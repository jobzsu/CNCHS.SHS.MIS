namespace SHS.StudentPortal.Common.Models;

public class InstructorViewModel
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

    public string AdvisorySection { get; set; }

    public List<Schedules> Schedules { get; set; }
}

public class Schedules
{
    public string Subject { get; set; }

    public string Days { get; set; }

    public string Time { get; set; }

    public string RoomNumber { get; set; }

    public string GradesSubmitted { get; set; }
}

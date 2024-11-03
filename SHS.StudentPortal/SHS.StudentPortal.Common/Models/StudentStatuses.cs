namespace SHS.StudentPortal.Common.Models;

public class StudentStatuses
{
    public string Id { get; private set; }
    public string Name { get; private set; }

    private StudentStatuses(string id, string name)
    {
        Id = id;
        Name = name;
    }

    public static StudentStatuses Pending => new("pending", "Pending");
    public static StudentStatuses ForAssessment => new("assessment", "For Assessment");
    public static StudentStatuses ForEnrollment => new("enrollment", "For Enrollment");
    public static StudentStatuses Regular => new("regular", "Regular");
    public static StudentStatuses Irregular => new("irregular", "Irregular");
    public static StudentStatuses Graduated => new("graduated", "Graduated");

    public static List<StudentStatuses> AllStatusList => 
        new() { Pending, ForAssessment, ForEnrollment, Regular, Irregular };

    public static List<StudentStatuses> AllForEnrollment =>
        new() { Regular, Irregular };

    public static StudentStatuses Get(string id)
    {
        return AllStatusList.FirstOrDefault(x => x.Id == id) ?? 
            throw new NullReferenceException("Student Status not found.");
    }
}

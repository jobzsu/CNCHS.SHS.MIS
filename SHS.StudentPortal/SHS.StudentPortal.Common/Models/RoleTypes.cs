namespace SHS.StudentPortal.Common.Models;

public class RoleTypes
{
    public readonly string Id;
    public readonly string Name;

    private RoleTypes(string id, string name)
    {
        Id = id;
        Name = name;
    }

    public static readonly RoleTypes Admin = new RoleTypes("admin", "Admin");
    public static readonly RoleTypes Instructor = new RoleTypes("instructor", "Instructor");
    public static readonly RoleTypes Student = new RoleTypes("student", "Student");

    public static List<RoleTypes> AllRoles => new List<RoleTypes> { Admin, Instructor, Student };

    public bool IsAdmin => this == Admin;
    public bool IsInstructor => this == Instructor;
    public bool IsStudent => this == Student;
}

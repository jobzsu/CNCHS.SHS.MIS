namespace SHS.StudentPortal.Common.Models;

public class DepartmentViewModel
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }

    public List<string> Instructors { get; set; }
}

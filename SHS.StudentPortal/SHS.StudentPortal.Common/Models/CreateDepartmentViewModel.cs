namespace SHS.StudentPortal.Common.Models;

public class CreateDepartmentViewModel
{
    public string Name { get; set; }

    public string Description { get; set; }

    public static CreateDepartmentViewModel New()
    {
        return new CreateDepartmentViewModel
        {
            Name = string.Empty,
            Description = string.Empty
        };
    }
}

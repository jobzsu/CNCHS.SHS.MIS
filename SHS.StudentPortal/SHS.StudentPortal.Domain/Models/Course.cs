using System.ComponentModel.DataAnnotations.Schema;

namespace SHS.StudentPortal.Domain.Models;
// ============= NOT USED ==============
public class Course : BaseModel<int>, IAuditable
{
    public string Name { get; set; }

    public string? Description { get; set; }

    public int DepartmentId { get; set; }

    [NotMapped]
    public Department Department { get; set; }

    [NotMapped]
    public ICollection<Subject> Subjects { get; set; }

    [NotMapped]
    public ICollection<StudentInfo> Students { get; set; }

    #region > IAuditable Properties
    public Guid? CreatedById { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? ModifiedById { get; set; }

    public DateTime? ModifiedDate { get; set; }
    #endregion

    public Course Create(string name,
        string? description,
        int departmentId,
        Guid createdById)
    {
        Name = name;
        Description = description;
        DepartmentId = departmentId;

        CreatedById = createdById;
        CreatedDate = DateTime.UtcNow;

        return this;
    }
}

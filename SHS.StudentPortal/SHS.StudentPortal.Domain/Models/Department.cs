using System.ComponentModel.DataAnnotations.Schema;

namespace SHS.StudentPortal.Domain.Models;

public class Department : BaseModel<int>, IAuditable
{
    public string Name { get; set; }

    public string? Description { get; set; }

    [NotMapped]
    public ICollection<InstructorInfo> Instructors { get; set; }

    [NotMapped]
    public ICollection<Course> Courses { get; set; }

    #region > IAuditable Properties
    public Guid? CreatedById { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? ModifiedById { get; set; }

    public DateTime? ModifiedDate { get; set; }
    #endregion
}

using System.ComponentModel.DataAnnotations.Schema;

namespace SHS.StudentPortal.Domain.Models;

public class InstructorInfo : BaseModel<Guid>, IAuditable
{
    public string EmployeeId { get; set; }

    public string Major { get; set; }

    public string ContactInformation { get; set; }

    public Guid UserId { get; set; }

    [NotMapped]
    public User User { get; set; }

    public int DepartmentId { get; set; }

    [NotMapped]
    public Department Department { get; set; }

    [NotMapped]
    public ICollection<Schedule> Schedules { get; set; }

    [NotMapped]
    public Section Section { get; set; }

    #region > IAuditable Properties
    public Guid? CreatedById { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? ModifiedById { get; set; }

    public DateTime? ModifiedDate { get; set; }
    #endregion

    public InstructorInfo Create(string employeeId, 
        string major,
        string contactInformation, 
        Guid userId, 
        int departmentId,
        Guid createdById)
    {
        EmployeeId = employeeId;
        Major = major;
        ContactInformation = contactInformation;
        UserId = userId;
        DepartmentId = departmentId;

        CreatedById = createdById;
        CreatedDate = DateTime.UtcNow;

        return this;
    }
}

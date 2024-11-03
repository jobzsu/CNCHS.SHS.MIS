using System.ComponentModel.DataAnnotations.Schema;

namespace SHS.StudentPortal.Domain.Models;

public class StudentSchedule : BaseModel<long>, IAuditable
{
    public Guid StudentId { get; set; }

    [NotMapped]
    public StudentInfo Student { get; set; }

    public long ScheduleId { get; set; }

    [NotMapped]
    public Schedule Schedule { get; set; }

    public string Semester { get; set; }

    public string AcademicYear { get; set; }

    #region > IAuditable Properties
    public Guid? CreatedById { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? ModifiedById { get; set; }

    public DateTime? ModifiedDate { get; set; }
    #endregion

    public StudentSchedule Create(Guid studentId,
        long scheduleId,
        string semester,
        string academicYear,
        Guid createdById)
    {
        StudentId = studentId;
        ScheduleId = scheduleId;
        Semester = semester;
        AcademicYear = academicYear;

        CreatedById = createdById;
        CreatedDate = DateTime.UtcNow;

        return this;
    }
}

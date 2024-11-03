using System.ComponentModel.DataAnnotations.Schema;

namespace SHS.StudentPortal.Domain.Models;

public class Schedule : BaseModel<long>, IAuditable
{
    public string Day { get; set; }

    public TimeOnly TimeStart { get; set; }

    public TimeOnly TimeEnd { get; set; }

    public string RoomNumber { get; set; }

    public string? Remarks { get; set; }

    public Guid InstructorId { get; set; }

    [NotMapped]
    public InstructorInfo Instructor { get; set; }

    public int SubjectId { get; set; }

    [NotMapped]
    public Subject Subject { get; set; }

    [NotMapped]
    public ICollection<StudentSchedule> StudentSchedules { get; set; }

    #region > IAuditable Properties
    public Guid? CreatedById { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? ModifiedById { get; set; }

    public DateTime? ModifiedDate { get; set; }
    #endregion

    public Schedule Create(string day,
        TimeOnly timeStart,
        TimeOnly timeEnd,
        string roomNumber,
        string? remarks,
        Guid instructorId,
        int subjectId,
        Guid createdById)
    {
        Day = day;
        TimeStart = timeStart;
        TimeEnd = timeEnd;
        RoomNumber = roomNumber;
        Remarks = remarks;
        InstructorId = instructorId;
        SubjectId = subjectId;

        CreatedById = createdById;
        CreatedDate = DateTime.UtcNow;

        return this;
    }

    public Schedule Update(string day,
        TimeOnly timeStart,
        TimeOnly timeEnd,
        string roomNumber,
        string? remarks,
        Guid instructorId,
        int subjectId,
        Guid updatedById)
    {
        Day = day;
        TimeStart = timeStart;
        TimeEnd = timeEnd;
        RoomNumber = roomNumber;
        Remarks = remarks;
        InstructorId = instructorId;
        SubjectId = subjectId;

        ModifiedById = updatedById;
        ModifiedDate = DateTime.UtcNow;

        return this;
    }
}

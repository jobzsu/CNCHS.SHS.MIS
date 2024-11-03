using System.ComponentModel.DataAnnotations.Schema;

namespace SHS.StudentPortal.Domain.Models;

public class Subject : BaseModel<int>, IAuditable
{
    public string Code { get; set; }

    public string Name { get; set; }

    public int Hours { get; set; }

    public int Days { get; set; }

    public string Description { get; set; }

    public int Units { get; set; }

    public string Semester { get; set; }

    public string AcademicYear { get; set; }

    public string TrackAndStrand { get; set; }

    [NotMapped]
    public ICollection<AcademicRecord> AcademicRecords { get; set; }

    [NotMapped]
    public ICollection<PreRequisite> PreRequisites { get; set; }

    [NotMapped]
    public ICollection<Schedule> Schedules { get; set; }

    #region > IAuditable Properties
    public Guid? CreatedById { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? ModifiedById { get; set; }

    public DateTime? ModifiedDate { get; set; }
    #endregion

    public Subject Create(string code,
        string name,
        int hours,
        int days,
        string description,
        int units,
        string semester,
        string academicYear,
        string trackAndStrand,
        Guid createdById)
    {
        Code = code;
        Name = name;
        Hours = hours;
        Days = days;
        Description = description;
        Units = units;
        Semester = semester;
        AcademicYear = academicYear;
        TrackAndStrand = trackAndStrand;

        CreatedById = createdById;
        CreatedDate = DateTime.UtcNow;

        return this;
    }
}

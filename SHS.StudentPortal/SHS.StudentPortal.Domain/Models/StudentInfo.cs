using System.ComponentModel.DataAnnotations.Schema;

namespace SHS.StudentPortal.Domain.Models;

public class StudentInfo : BaseModel<Guid>, IAuditable
{
    public int IdNumber { get; set; }

    public string StudentStatus { get; set; }

    public int YearLevel { get; set; }

    public Guid SectionId { get; set; }

    [NotMapped]
    public Section Section { get; set; }

    public string TrackAndStrand { get; set; }

    public Guid UserId { get; set; }

    [NotMapped]
    public User User { get; set; }

    [NotMapped]
    public ICollection<AcademicRecord> AcademicRecords { get; set; }

    [NotMapped]
    public ICollection<ExternalAcademicRecord> ExternalAcademicRecords { get; set; }

    [NotMapped]
    public ICollection<StudentSchedule> StudentSchedules { get; set; }

    public string Gender { get; set; }

    public DateOnly DateOfBirth { get; set; }

    public string PlaceOfBirth { get; set; }

    public int Age { get; set; }

    public string CivilStatus { get; set; }

    public string Nationality { get; set; }

    public string Religion { get; set; }

    public string ContactInformation { get; set; }

    public string Address { get; set; }

    #region > IAuditable Properties
    public Guid? CreatedById { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? ModifiedById { get; set; }

    public DateTime? ModifiedDate { get; set; }
    #endregion

    public StudentInfo Create(string status,
        int yearLevel,
        Guid sectionId,
        string trackAndStrand,
        Guid userId,
        string gender,
        DateOnly dateOfBirth,
        string placeOfBirth,
        string civilStatus,
        string nationality,
        string religion,
        string contactInformation,
        string address,
        Guid createdById)
    {
        StudentStatus = status;

        YearLevel = yearLevel;
        SectionId = sectionId;
        TrackAndStrand = trackAndStrand;
        UserId = userId;

        Gender = gender;
        DateOfBirth = dateOfBirth;
        PlaceOfBirth = placeOfBirth;
        var initialAge = DateTime.UtcNow.Year - dateOfBirth.Year;
        Age = DateTime.UtcNow.DayOfYear < dateOfBirth.DayOfYear ? initialAge - 1 : initialAge;
        CivilStatus = civilStatus;
        Nationality = nationality;
        Religion = religion;
        ContactInformation = contactInformation;
        Address = address;

        CreatedById = createdById;
        CreatedDate = DateTime.UtcNow;

        return this;
    }

    public StudentInfo UpdateStudentStatus(string status,
        Guid modifiedById)
    {
        StudentStatus = status;

        ModifiedById = modifiedById;
        ModifiedDate = DateTime.UtcNow;

        return this;
    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace SHS.StudentPortal.Domain.Models;

public class AcademicRecord : BaseModel<long>, IAuditable
{
    public string Rating { get; set; }

    public Guid StudentId { get; set; }

    [NotMapped]
    public StudentInfo Student { get; set; }

    public int SubjectId { get; set; }

    public string OtherSubjectName { get; set; }

    [NotMapped]
    public Subject Subject { get; set; }

    public string Semester { get; set; }

    public string AcademicYear { get; set; }

    public Guid? EncodedById { get; set; }

    public DateTime? EncodedDate { get; set; }

    public Guid? VerifiedById { get; set; }

    public DateTime? VerifiedDate { get; set; }

    #region > IAuditable Properties
    public Guid? CreatedById { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? ModifiedById { get; set; }

    public DateTime? ModifiedDate { get; set; }
    #endregion

    public AcademicRecord Create(string academicYear,
        string semester,
        int subjectId,
        string otherSubjectName,
        string rating,
        Guid studentId,
        Guid encodedById,
        Guid createdById)
    {
        AcademicYear = academicYear;
        Semester = semester;
        SubjectId = subjectId;
        OtherSubjectName = otherSubjectName;
        Rating = rating;

        StudentId = studentId;

        EncodedById = encodedById;
        EncodedDate = DateTime.UtcNow;

        CreatedById = createdById;
        CreatedDate = DateTime.UtcNow;

        return this;
    }

    public AcademicRecord SetAsVerified(Guid verifiedById)
    {
        VerifiedById = verifiedById;
        VerifiedDate = DateTime.UtcNow;

        ModifiedById = verifiedById;
        ModifiedDate = DateTime.UtcNow;

        return this;
    }
}

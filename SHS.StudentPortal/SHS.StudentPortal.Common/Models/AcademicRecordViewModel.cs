using System.ComponentModel.DataAnnotations;

namespace SHS.StudentPortal.Common.Models;

public class AcademicRecordViewModel
{
    public long Id { get; set; }

    [Required(ErrorMessage = "Please enter a rating for this subject")]
    public string Rating { get; set; }

    [Required(ErrorMessage = "Please enter a semester")]
    public string Semester { get; set; }

    [Required(ErrorMessage = "Please enter a subject name")]
    public string SubjectName { get; set; }

    [Required(ErrorMessage = "Please enter an academic year")]
    public string AcademicYear { get; set; }

    public string EncodedBy { get; set; }

    public string EncodedDate { get; set; }

    public string VerifiedBy { get; set; }

    public string VerifiedDate { get; set; }

    public int TempId { get; set; }
}

public class AcademicRecordAdminViewDTO
{
    public long Id { get; set; }

    [Required(ErrorMessage = "Please enter a rating for this subject")]
    public string Rating { get; set; }

    [Required(ErrorMessage = "Please enter a semester")]
    public string Semester { get; set; }

    public int SubjectId { get; set; }

    [Required(ErrorMessage = "Please enter a subject name")]
    public string SubjectName { get; set; }

    [Required(ErrorMessage = "Please enter an academic year")]
    public string AcademicYear { get; set; }

    public string EncodedBy { get; set; }

    public Guid EncodedById { get; set; }

    public DateTime? EncodedDate { get; set; }

    public string VerifiedBy { get; set; }

    public Guid? VerifiedById { get; set; }

    public DateTime? VerifiedDate { get; set; }

    public int TempId { get; set; }
}

public class NewAcademicRecordDTO
{
    public string AcademicYear { get; set; }

    public string Semester { get; set; }

    public int SubjectId { get; set; }

    public string SubjectName { get; set; }

    public string Rating { get; set; }
}

public class UpsertAcademicRecordDTO
{
    public long Id { get; set; }

    public string AcademicYear { get; set; }

    public string Semester { get; set; }

    public string Rating { get; set; }

    public Guid StudentId { get; set; }

    public int SubjectId { get; set; }

    public string OtherSubjectName { get; set; }

    public Guid CreatedById { get; set; }
}
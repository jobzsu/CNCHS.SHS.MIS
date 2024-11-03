using System.ComponentModel.DataAnnotations;

namespace SHS.StudentPortal.Common.Models;

public class ExternalAcademicRecordViewModel
{
    [Required(ErrorMessage = "Please enter a subject name")]
    public string SubjectName { get; set; }

    [Required(ErrorMessage = "Please enter a rating for this subject")]
    public string Rating { get; set; }

    [Required(ErrorMessage = "Please enter a semester")]
    public string Semester { get; set; }

    [Required(ErrorMessage = "Please enter an academic year")]
    public string AcademicYear { get; set; }

    public int TempId { get; set; }
}

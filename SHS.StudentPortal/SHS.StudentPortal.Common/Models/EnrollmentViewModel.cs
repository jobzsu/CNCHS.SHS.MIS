namespace SHS.StudentPortal.Common.Models;

public class EnrollmentViewModel
{
    public Guid StudentId { get; set; }

    public string DesignatedGradeLevel { get; set; }

    public string DesignatedSection { get; set; }

    public string DesignatedStatus { get; set; }

    public List<long> SelectedSchedules { get; set; }
}

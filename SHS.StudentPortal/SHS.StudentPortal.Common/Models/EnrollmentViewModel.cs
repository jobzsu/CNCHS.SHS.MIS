namespace SHS.StudentPortal.Common.Models;

public class EnrollmentViewModel
{
    public Guid StudentId { get; set; }

    public int DesignatedGradeLevel { get; set; }

    public Guid DesignatedSectionId { get; set; }

    public string DesignatedStatus { get; set; }

    public List<long> SelectedSchedules { get; set; }

    public static EnrollmentViewModel New()
    {
        return new EnrollmentViewModel()
        {
            StudentId = Guid.Empty,
            DesignatedGradeLevel = 0,
            DesignatedSectionId = Guid.Empty,
            DesignatedStatus = string.Empty,
            SelectedSchedules = new()
        };
    }
}

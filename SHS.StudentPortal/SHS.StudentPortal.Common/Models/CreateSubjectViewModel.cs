namespace SHS.StudentPortal.Common.Models;

public class CreateSubjectViewModel
{
    public string Code { get; set; }

    public string Name { get; set; }

    public string TrackId { get; set; }

    public string StrandId { get; set; }

    public int Hours { get; set; }

    public int Days { get; set; }

    public string Description { get; set; }

    public int Units { get; set; }

    public string Semester { get; set; }

    public string AcademicYear { get; set; }
}

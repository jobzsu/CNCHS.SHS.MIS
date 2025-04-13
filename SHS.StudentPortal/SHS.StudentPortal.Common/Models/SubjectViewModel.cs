namespace SHS.StudentPortal.Common.Models;

public class SubjectViewModel
{
    public int Id { get; set; }

    public string Code { get; set; }

    public string Name { get; set; }

    public string? TrackId { get; set; }

    public string? StrandId { get; set; }

    public int Hours { get; set; }

    public int Days { get; set; }

    public string Description { get; set; }

    public int Units { get; set; }

    public string Semester { get; set; }

    public string AcademicYear { get; set; }

    public static SubjectViewModel New()
    {
        return new SubjectViewModel
        {
            Id = 0,
            Code = string.Empty,
            Name = string.Empty,
            TrackId = null,
            StrandId = null,
            Hours = 0,
            Days = 0,
            Description = string.Empty,
            Units = 0,
            Semester = "1st",
            AcademicYear = "2025-2026"
        };
    }
}

using System.ComponentModel.DataAnnotations;

namespace SHS.StudentPortal.Common.Models;

public class EnrollStudentViewModel
{
    public Guid StudentId { get; set; }

    public string CurrentSemester { get; set; }

    public string CurrentAcademicYear { get; set; }

    public string TrackAndStrand { get; set; }

    public List<KeyValuePair<Guid, string>> SectionList { get; set; }

    public List<EnrollStudentScheduleListViewModel> Schedules { get; set; }
}

public class EnrollStudentScheduleListViewModel
{
    public long ScheduleId { get; set; }

    public string Subject { get; set; }

    public string Instructor { get; set; }

    public string Days { get; set; }

    public string Time { get; set; }

    public string Room { get; set; }

    public List<string> PreReqSubjects { get; set; }

    public bool IsSelected { get; set; }
}

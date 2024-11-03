using SHS.StudentPortal.Common.Models;

namespace SHS.StudentPortal.Common.Utilities.Models;

public class SchedulePaginatedList
{
    public PaginatedList<ScheduleListViewModel> ScheduleList { get; set; }

    public string CurrentSemester { get; set; }

    public string CurrentAcademicYear { get; set; }
}

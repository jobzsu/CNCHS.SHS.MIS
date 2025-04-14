namespace SHS.StudentPortal.Common.Models;

public class NewScheduleViewModel
{
    public string SubjectId { get; set; }

    public List<KeyValuePair<int, string>> SubjectList { get; set; }

    public string InstructorId { get; set; }

    public List<KeyValuePair<Guid, string>> InstructorList { get; set; }

    public string Room { get; set; }

    public string Days { get; set; }

    public string TimeStartHour { get; set; }

    public string TimeStartMinute { get; set; }

    public string TimeStartAMPM { get; set; }

    public string TimeEndHour { get; set; }

    public string TimeEndMinute { get; set; }

    public string TimeEndAMPM { get; set; }

    public string Remarks { get; set; }
}

public class UpsertScheduleViewModel
{
    public long Id { get; set; }

    public int? SubjectId { get; set; }

    public Guid? InstructorId { get; set; }

    public string Room { get; set; }

    public List<string>? Days { get; set; }

    public DateTime? TimeStart { get; set; }

    public DateTime? TimeEnd { get; set; }

    public string Remarks { get; set; }

    public static UpsertScheduleViewModel New()
    {
        return new UpsertScheduleViewModel()
        {
            Id = 0,
            SubjectId = null,
            InstructorId = null,
            Room = string.Empty,
            Days = null,
            TimeStart = null,
            TimeEnd = null,
            Remarks = string.Empty,
        };
    }
}

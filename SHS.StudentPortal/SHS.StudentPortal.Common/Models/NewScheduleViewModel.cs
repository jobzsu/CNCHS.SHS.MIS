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

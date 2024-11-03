namespace SHS.StudentPortal.Common.Models;

public class CreateScheduleViewModel
{
    public string SubjectId { get; set; }

    public string InstructorId { get; set; }

    public string Room { get; set; }

    public string Day { get; set; }

    public string TimeStartHour { get; set; }

    public string TimeStartMin { get; set; }

    public string TimeStartAMPM { get; set; }

    public string TimeEndHour { get; set; }

    public string TimeEndMin { get; set; }

    public string TimeEndAMPM { get; set; }

    public string Remarks { get; set; }
}

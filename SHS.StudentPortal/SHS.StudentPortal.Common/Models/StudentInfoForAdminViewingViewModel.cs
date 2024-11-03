namespace SHS.StudentPortal.Common.Models;

public class StudentInfoForAdminViewingViewModel
{
    public Guid Id { get; set; }


    #region > Academic Info

    public string StudentIdNum { get; set; }

    public string Status { get; set; }

    public string Track { get; set; }

    public string Strand { get; set; }

    public int YearLevel { get; set; }

    public Guid SectionId { get; set; }

    public List<KeyValuePair<Guid, string>> SectionList { get; set; }

    #endregion

    #region > Login Info
    public string Username { get; set; }

    public string LastLogin { get; set; }
    #endregion

    #region > Personal Info

    public string FirstName { get; set; }

    public string? MiddleName { get; set; }

    public string LastName { get; set; }

    public string Gender { get; set; }

    public string DateOfBirth { get; set; }

    public string PlaceOfBirth { get; set; }

    public string CivilStatus { get; set; }

    public string Nationality { get; set; }

    public string Religion { get; set; }

    public string ContactInfo { get; set; }

    public string Address { get; set; }

    #endregion

    #region > Prev Academic Records
    public List<ExternalAcademicRecordViewModel> PreviousAcademicRecords { get; set; }
    #endregion

    #region > Schedules

    public CurrentSchedulesAssignedViewModel CurrentSchedules { get; set; }

    #endregion
}

public class CurrentSchedulesAssignedViewModel
{
    public string Semester { get; set; }

    public string AcademicYear { get; set; }

    public List<ScheduleListViewModel> Schedules { get; set; }
}

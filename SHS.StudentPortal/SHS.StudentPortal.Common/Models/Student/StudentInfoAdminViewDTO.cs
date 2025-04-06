namespace SHS.StudentPortal.Common.Models.Student;

public class StudentInfoAdminViewDTO
{
    public Guid Id { get; set; }

    #region > Academic Info

    public string StudentIdNum { get; set; }

    public string Status { get; set; }

    public string Track { get; set; }

    public string Strand { get; set; }

    public int YearLevel { get; set; }

    public Guid SectionId { get; set; }

    #endregion

    #region > Login Info

    public string Username { get; set; }

    public DateTime? LastLogin { get; set; }

    #endregion

    #region > Personal Info

    public string FirstName { get; set; }

    public string? MiddleName { get; set; }

    public string LastName { get; set; }

    public string Gender { get; set; }

    public DateOnly DateOfBirth { get; set; }

    public string PlaceOfBirth { get; set; }

    public string CivilStatus { get; set; }

    public string Nationality { get; set; }

    public string Religion { get; set; }

    public string ContactInfo { get; set; }

    public string Address { get; set; }

    #endregion

    #region > Prev Academic Records
    public List<AcademicRecordAdminViewDTO> AcademicRecords { get; set; }
    #endregion

    #region > Schedules

    public CurrentSchedulesAssignedViewModel CurrentSchedules { get; set; }

    #endregion

    public static StudentInfoAdminViewDTO New()
    {
        return new StudentInfoAdminViewDTO()
        {
            Id = Guid.Empty,
            StudentIdNum = "NEW",
            Status = string.Empty,
            Track = string.Empty,
            Strand = string.Empty,
            Username = string.Empty,
            LastLogin = null,
            FirstName = string.Empty,
            MiddleName = null,
            LastName = string.Empty,
            Gender = string.Empty,
            DateOfBirth = DateOnly.FromDateTime(DateTime.Now),
            PlaceOfBirth = string.Empty,
            CivilStatus = string.Empty,
            Nationality = string.Empty,
            Religion = string.Empty,
            ContactInfo = string.Empty,
            Address = string.Empty,
            AcademicRecords = new(),
            CurrentSchedules = new()
            {
                AcademicYear = string.Empty,
                Semester = string.Empty,
                Schedules = new(),
            }
        };
    }
}

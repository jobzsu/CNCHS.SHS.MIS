using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.App.Abstractions.Repositories;

public interface IStudentScheduleRepository
{
    Task<StudentSchedule> CreateStudentSchedule(StudentSchedule studentSchedule, CancellationToken cancellationToken = default);

    Task<List<StudentSchedule>?> GetByStudentIdCurrentSemesterAcademicYear(Guid studentId,
        string currentSemester,
        string currentAcademicYear,
        bool shouldTrack = false,
        CancellationToken cancellationToken = default);
}

using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.App.Abstractions.Repositories;

public interface ICourseRepository
{
    Task<Course?> GetByName(string courseName, bool shouldTrack = false, CancellationToken cancellationToken = default);

    Task<List<Course>?> GetAllCourses(bool shouldTrack = false, CancellationToken cancellationToken = default);
}

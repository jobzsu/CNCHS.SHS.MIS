using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.App.Abstractions.Repositories;

public interface ISubjectRepository
{
    Task<List<Subject>?> GetAllSubjectBySemester(string semester, bool shouldTrack = false, CancellationToken cancellationToken = default);

    Task<Subject?> GetSubjectById(int id, bool shouldTrack = false, CancellationToken cancellationToken = default);
}

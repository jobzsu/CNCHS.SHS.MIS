using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.App.Abstractions.Repositories;

public interface ISubjectRepository
{
    Task<Subject> UpdateSubject(Subject subject, CancellationToken cancellationToken = default);

    Task<List<Subject>?> GetAllSubjectBySemester(string semester, bool shouldTrack = false, CancellationToken cancellationToken = default);
    
    Task<List<Subject>?> GetAllSubjects(CancellationToken cancellationToken = default, bool shouldTrack = false);

    Task<Subject?> GetSubjectById(int id, bool shouldTrack = false, CancellationToken cancellationToken = default);

    Task<Subject?> GetSubjectByName(string name, bool shouldTrack = false, CancellationToken cancellationToken = default);

    Task<Subject?> GetSubjectByCode(string code, bool shouldTrack = false, CancellationToken cancellationToken = default);

    Task<List<Subject>?> GetListViaFilter(string? keyword,
        string? track,
        string? strand,
        CancellationToken cancellationToken = default);
}

using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.App.Abstractions.Repositories;

public interface IPreRequisiteRepository
{
    Task<List<PreRequisite>?> GetAllBySubjectId(int parentSubjectId, bool shouldTrack = false, CancellationToken cancellationToken = default);

    Task<List<PreRequisite>?> GetAllList(CancellationToken cancellationToken = default, bool shouldTrack = false);

    Task<List<PreRequisite>?> GetAllListForSubjects(List<int> subjectIds, CancellationToken cancellationToken = default, bool shouldTrack = false);
}

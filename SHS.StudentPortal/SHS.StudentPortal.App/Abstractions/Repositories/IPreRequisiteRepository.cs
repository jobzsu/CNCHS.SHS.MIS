using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.App.Abstractions.Repositories;

public interface IPreRequisiteRepository
{
    Task<List<PreRequisite>?> GetAllBySubjectId(int parentSubjectId, bool shouldTrack = false, CancellationToken cancellationToken = default);
}

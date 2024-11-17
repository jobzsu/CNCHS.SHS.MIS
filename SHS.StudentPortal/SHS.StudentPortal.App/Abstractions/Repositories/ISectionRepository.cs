using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.App.Abstractions.Repositories;

public interface ISectionRepository
{
    Task<Section?> GetSectionByName(string name, bool shouldTrack = false, CancellationToken cancellationToken = default);

    Task<List<Section>?> GetAllSections(bool includeNotApplicable = false, bool shouldTrack = false, CancellationToken cancellationToken = default);

    Task<List<Section>?> GetAvailableSections(Guid instructorId, bool shouldTrack = false, CancellationToken cancellationToken = default);

    Task<Section?> GetById(Guid id, bool shouldTrack = false, CancellationToken cancellationToken = default);

    Task<Section> UpdateSection(Section section, CancellationToken cancellationToken = default);

    Task<Section?> GetByAdviserId(Guid adviserId, bool shouldTrack = false, CancellationToken cancellationToken = default);
}

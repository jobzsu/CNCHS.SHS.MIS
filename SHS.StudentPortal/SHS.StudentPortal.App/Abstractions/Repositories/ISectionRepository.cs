﻿using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.App.Abstractions.Repositories;

public interface ISectionRepository
{
    Task<Section?> GetSectionByName(string name, bool shouldTrack = false, CancellationToken cancellationToken = default);

    Task<List<Section>?> GetAllSections(bool shouldTrack = false, CancellationToken cancellationToken = default);

    Task<Section?> GetById(Guid id, bool shouldTrack = false, CancellationToken cancellationToken = default);
}

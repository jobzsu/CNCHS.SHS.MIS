﻿using Microsoft.EntityFrameworkCore;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.Database.Repositories;

public class PreRequisiteRepository : BaseRepository<PreRequisite>, IPreRequisiteRepository
{
    public PreRequisiteRepository(AppDbContext appDbContext) : base(appDbContext) { }

    public async Task<List<PreRequisite>?> GetAllBySubjectId(int subjectId, bool shouldTrack = false, CancellationToken cancellationToken = default)
    {
        return await (shouldTrack ?
            GetAll()
            .AsSplitQuery()
            .Where(x => x.SubjectId == subjectId)
            .ToListAsync(cancellationToken) :
            GetAll()
            .AsNoTracking()
            .AsSplitQuery()
            .Where(x => x.SubjectId == subjectId)
            .ToListAsync(cancellationToken));
    }
}
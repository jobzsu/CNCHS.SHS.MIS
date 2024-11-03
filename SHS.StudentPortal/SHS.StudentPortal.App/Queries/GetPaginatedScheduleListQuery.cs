using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.Common.Utilities.Models;

namespace SHS.StudentPortal.App.Queries;

public sealed record GetPaginatedScheduleListQuery(SchedulePaginationFilter filter)
    : IQuery<SchedulePaginatedList>;

using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.Common.Models;
using SHS.StudentPortal.Common.Utilities.Models;

namespace SHS.StudentPortal.App.Queries;

public sealed record GetPaginatedStudentListQuery(StudentPaginationFilter filter) :
    IQuery<PaginatedList<StudentListViewModel>>;

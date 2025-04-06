using SHS.StudentPortal.App.Abstractions.Messaging;

namespace SHS.StudentPortal.App.Queries;

public sealed record GetCurrentSemesterAndAcademicYearQuery
    : IQuery<Tuple<string, string>>;

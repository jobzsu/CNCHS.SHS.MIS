using SHS.StudentPortal.App.Abstractions.Messaging;

namespace SHS.StudentPortal.App.Queries;

public sealed record GetDepartmentDropdownListQuery
    : IQuery<List<KeyValuePair<int, string>>>;
